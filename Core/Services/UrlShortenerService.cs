using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace BusinessLogic.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly AppDbContext _dbContext;

        public UrlShortenerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShortenUrlResultDto> TryShortenAsync(string originalUrl, string userId, HttpContext httpContext)
        {
            var existing = await _dbContext.Urls
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl && u.UserId == userId);

            if (existing != null)
            {
                return new ShortenUrlResultDto { success = false, message = "This URL already exists." };
            }

            var shortKey = GenerateShortUrl();
            var newUrl = new Url
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortKey,
                UserId = userId
            };

            await _dbContext.Urls.AddAsync(newUrl);
            await _dbContext.SaveChangesAsync();

            return new ShortenUrlResultDto
            {
                success = true,
                id = newUrl.Id,
                originalUrl = newUrl.OriginalUrl,
                shortenedUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{shortKey}",
                userId = newUrl.UserId
            };
        }


        public async Task<string?> GetOriginalUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
        {
            var url = await _dbContext.Urls
                .AsNoTracking()
                .Where(u => u.ShortenedUrl == shortUrl)
                .Select(u => u.OriginalUrl)
                .FirstOrDefaultAsync(cancellationToken);
            return url;
        }

        private static string GenerateShortUrl()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var shortUrl = new char[6];
            for (var i = 0; i < shortUrl.Length; i++)
            {
                shortUrl[i] = chars[random.Next(chars.Length)];
            }

            return new string(shortUrl);
        }
    }
}
