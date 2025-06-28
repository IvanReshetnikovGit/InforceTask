using BusinessLogic.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IUrlShortenerService
    {
        public Task<ShortenUrlResult> TryShortenAsync(string originalUrl, string userId, HttpContext httpContext);
        public Task<string?> GetOriginalUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
    }
}
