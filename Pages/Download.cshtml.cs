using clipy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace clipy.Pages;

public class DownloadModel(
    AppDbContext db,
    IFileStorageService storage
) : PageModel
{
    public async Task<IActionResult> OnGetAsync(string code)
    {
        var note = await db.Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Code == code);

        if (note == null || string.IsNullOrEmpty(note.FileName))
            return NotFound();

        var stream = await storage.GetFileStreamAsync(note.FileName);

        return File(stream, "application/octet-stream", note.OriginalFileName);
    }
}