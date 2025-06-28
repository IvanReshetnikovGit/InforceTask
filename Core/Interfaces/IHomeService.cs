using DataAccess.Entities;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces;
public interface IHomeService
{
    public Task<List<Url>> GetUrls();
    public Task DeleteUrlAsync(int id);
    public Task<UrlInfoDto?> GetUrlInfoAsync(int id, HttpContext httpContext);
}