using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace clipy.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public int CurrentYear { get; private set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        CurrentYear = DateTime.Now.Year;
    }
}
