using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InforceTask.Pages;

public class AboutModel : PageModel
{
    private readonly IWebHostEnvironment _env;
    private readonly string _filePath;

    public AboutModel(IWebHostEnvironment env)
    {
        _env = env;
        _filePath = Path.Combine(_env.WebRootPath, "data", "about.txt");
    }

    [BindProperty]
    public string Content { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public bool Edit { get; set; }

    public bool IsEditMode { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        IsEditMode = Edit && User.IsInRole("Admin");

        if (System.IO.File.Exists(_filePath))
            Content = await System.IO.File.ReadAllTextAsync(_filePath);
        else
            Content = "No content yet.";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!User.IsInRole("Admin"))
            return Forbid();

        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await System.IO.File.WriteAllTextAsync(_filePath, Content);

        return RedirectToPage("/About");
    }
}
