using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services;

public class HomeService : IHomeService
{
    private readonly AppDbContext _context;
    public HomeService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Url>> GetUrls()
    {
        return await _context.Urls.ToListAsync();
    }
}