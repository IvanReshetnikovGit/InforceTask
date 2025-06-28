using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InforceTask.Models;
using DataAccess.Data;
using BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Identity;

namespace InforceTask.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHomeService _homeService;
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(AppDbContext context, IHomeService homeService, IUrlShortenerService urlShortenerService, UserManager<IdentityUser> userManager)
    {
        _homeService = homeService;
        _context = context;
        _urlShortenerService = urlShortenerService;
        _userManager = userManager;

    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpDelete]
    public async Task<JsonResult> DeleteUrl(int id)
    {
        try
        {
            await _homeService.DeleteUrlAsync(id);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        
        return Json(new { success = true });
    }
    [HttpPost]
    public async Task<JsonResult> ShortenUrl([FromBody] ShortenUrlResult request)
    {
        var result = await _urlShortenerService.TryShortenAsync(request.originalUrl, request.userId, HttpContext);

        return Json(result);
    }

    [HttpGet("/{shortKey}")]
    public async Task<IActionResult> RedirectToOriginal(string shortKey)
    {
        var url = await _context.Urls
            .FirstOrDefaultAsync(u => u.ShortenedUrl == shortKey);

        if (url == null)
            return NotFound();

        return Redirect(url.OriginalUrl);
    }


    [HttpGet]
    public IActionResult GetUrls()
    {
        var urls = _context.Urls
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.OriginalUrl,
                u.UserId,
                ShortenedUrl = $"{Request.Scheme}://{Request.Host}/{u.ShortenedUrl}",
            })
            .ToList();

        return Json(urls);
    }
    [HttpGet("Home/GetUrlInfo/{id}")]
    public async Task<IActionResult> GetUrlInfo(int id)
    {
        var info = await _homeService.GetUrlInfoAsync(id, HttpContext);

        if (info == null)
            return NotFound();

        return Json(info);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
