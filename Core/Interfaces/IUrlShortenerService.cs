using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IUrlShortenerService
    {
        public Task<ShortenUrlResultDto> TryShortenAsync(string originalUrl, string userId, HttpContext httpContext);
        public Task<string?> GetOriginalUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
    }
}
