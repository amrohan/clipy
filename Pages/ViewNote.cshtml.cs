using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class ViewNoteModel : PageModel
{
    private readonly AppDbContext _db;
    public ViewNoteModel(AppDbContext db) => _db = db;

    public Note? Note { get; set; }
    public bool ShowConfirmation { get; set; }
    public bool JustViewedDeletableNote { get; set; }
    public bool RequiresPassword { get; set; }
    public bool PasswordAttempted { get; set; }
    public bool AttemptedAndNotFound { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Code { get; set; }

    [BindProperty]
    public string? SubmittedPassword { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            return Page();
        }

        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);


        note = await CleanupAndValidateNoteAsync(note);

        if (note == null)
        {
            AttemptedAndNotFound = true;
            return Page();
        }


        if (!string.IsNullOrEmpty(note.Password))
        {
            RequiresPassword = true;
            return Page();
        }


        if (note.DeleteAfterView && !note.Viewed)
        {
            ShowConfirmation = true;
            return Page();
        }


        Note = note;
        return Page();
    }
    public async Task<IActionResult> OnPostConfirmAsync()
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);
        if (note != null && note.DeleteAfterView && string.IsNullOrEmpty(note.Password))
        {
            note.Viewed = true;
            await _db.SaveChangesAsync();
            JustViewedDeletableNote = true;
            Note = note;
        }
        else
        {
            AttemptedAndNotFound = true;
        }
        return Page();
    }
    public async Task<IActionResult> OnPostPasswordAsync()
    {
        if (string.IsNullOrEmpty(SubmittedPassword)) return Page();

        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);
        note = await CleanupAndValidateNoteAsync(note);

        if (note == null || string.IsNullOrEmpty(note.Password))
        {
            AttemptedAndNotFound = true;
            return Page();
        }


        if (!BCrypt.Net.BCrypt.Verify(SubmittedPassword, note.Password))
        {
            RequiresPassword = true;
            PasswordAttempted = true;
            return Page();
        }



        if (note.DeleteAfterView)
        {
            note.Viewed = true;
            await _db.SaveChangesAsync();
            JustViewedDeletableNote = true;
        }

        Note = note;
        return Page();
    }
    private async Task<Note?> CleanupAndValidateNoteAsync(Note? note)
    {
        if (note == null) return null;

        bool shouldDelete = false;


        if (note.ExpiryDateUtc.HasValue && note.ExpiryDateUtc.Value < DateTime.UtcNow)
        {
            shouldDelete = true;
        }


        if (note.DeleteAfterView && note.Viewed)
        {
            shouldDelete = true;
        }

        if (shouldDelete)
        {
            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
            return null;
        }

        return note;
    }
}