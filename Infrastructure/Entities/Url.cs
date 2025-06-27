using DataAccess.Interfaces;
namespace DataAccess.Entities;

public class Url : IEntity
{
    public int Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortenedUrl { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}