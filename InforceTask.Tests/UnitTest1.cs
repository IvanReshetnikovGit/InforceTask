using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace InforceTask.Tests;
public class UrlShortenerServiceTests
{
    [Fact]
    public async Task TryShortenAsync_ExistingUrl_ReturnsError()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "UrlTestDb")
            .Options;

        using var dbContext = new AppDbContext(options);

        var userId = "user-123";
        var url = new Url
        {
            OriginalUrl = "https://google.com",
            ShortenedUrl = "abc123",
            UserId = userId
        };

        await dbContext.Urls.AddAsync(url);
        await dbContext.SaveChangesAsync();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost");

        var service = new UrlShortenerService(dbContext);

        var result = await service.TryShortenAsync("https://google.com", userId, httpContext);

        Assert.False(result.success);
        Assert.Equal("This URL already exists.", result.message);
    }
}
