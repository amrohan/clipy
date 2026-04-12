using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using clipy.Services;

namespace clipy.Pages;

public class MyNotesModel(
    AppDbContext db,
    UserManager<IdentityUser> userManager,
    IEncryptionService encryptionService)
    : PageModel
{
    public List<NoteViewModel> Notes { get; set; } = [];

    [BindProperty(SupportsGet = true)] public string? SearchQuery { get; set; }

    [BindProperty(SupportsGet = true)] public int P { get; set; } = 1;

    public int TotalPages { get; set; }

    public class NoteViewModel
    {
        public Note Note { get; set; } = null!;
        public string? DecryptedContent { get; set; }
        public bool IsExpired { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = userManager.GetUserId(User);
        if (userId == null) return RedirectToPage("/Account/Login");

        const int pageSize = 12;

        var query = db.Notes.Where(n => n.UserId == userId);

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var s = $"%{SearchQuery}%";

            query = query.Where(n =>
                EF.Functions.Like(n.Code, s) ||
                (n.OriginalFileName != null && EF.Functions.Like(n.OriginalFileName, s)) ||
                (!n.IsEncrypted && EF.Functions.Like(n.Content, s))
            );
        }

        var totalItems = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        if (P < 1) P = 1;
        if (P > TotalPages && TotalPages > 0) P = TotalPages;

        var rawNotes = await query
            .OrderByDescending(n => n.CreatedDateUtc)
            .Skip((P - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        Notes = rawNotes.Select(n => new NoteViewModel
        {
            Note = n,
            DecryptedContent = n.IsEncrypted ? encryptionService.Decrypt(n.Content) : n.Content,
            IsExpired = n.ExpiryDateUtc != null && n.ExpiryDateUtc < DateTime.UtcNow
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var userId = userManager.GetUserId(User);
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (note != null)
        {
            db.Notes.Remove(note);
            await db.SaveChangesAsync();
        }

        return RedirectToPage(new { SearchQuery, P });
    }
}