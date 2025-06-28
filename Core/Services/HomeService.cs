using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services;

public class HomeService : IHomeService
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public HomeService(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<List<Url>> GetUrls()
    {
        return await _context.Urls.ToListAsync();
    }
    public async Task DeleteUrlAsync(int id)
    {
        var url = await _context.Urls.FindAsync(id);
        if (url == null)
        {
            throw new KeyNotFoundException("URL not found");
        }
        
        _context.Urls.Remove(url);
        await _context.SaveChangesAsync();
    }

    public async Task<UrlInfoDto?> GetUrlInfoAsync(int id, HttpContext httpContext)
    {
        var url = await _context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (url == null)
            return null;

        var userEmail = await _userManager.Users
            .Where(u => u.Id == url.UserId)
            .Select(u => u.Email)
            .FirstOrDefaultAsync();

        return new UrlInfoDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortenedUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{url.ShortenedUrl}",
            UserEmail = userEmail,
            Created = url.CreatedAt
        };
    }

}