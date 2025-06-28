namespace BusinessLogic.DTOs;
public class ShortenUrlResult
{
    public bool success { get; set; }
    public string message { get; set; }
    public int? id { get; set; }
    public string originalUrl { get; set; }
    public string shortenedUrl { get; set; }
    public string userId { get; set; }
}

