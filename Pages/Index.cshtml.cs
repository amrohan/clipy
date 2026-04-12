using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace clipy.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    public int CurrentYear { get; private set; }

    public IActionResult OnGet()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToPage("/MyNotes");
        }

        CurrentYear = DateTime.Now.Year;

        return Page();
    }
}