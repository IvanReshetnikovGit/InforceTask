namespace BusinessLogic.DTOs;
public class UrlInfoDto
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortenedUrl { get; set; }
    public string UserEmail { get; set; }
    public DateTime? Created { get; set; }
}
