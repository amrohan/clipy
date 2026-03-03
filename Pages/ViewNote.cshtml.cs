using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class ViewNoteModel(AppDbContext db, IEncryptionService encryptionService) : PageModel
{

    public Note? Note { get; set; }
    public bool ShowConfirmation { get; set; }
    public bool JustViewedDeletableNote { get; set; }
    public bool RequiresPassword { get; set; }
    public bool PasswordAttempted { get; set; }
    public bool AttemptedAndNotFound { get; set; }
    public int CurrentYear { get; private set; }

    [BindProperty(SupportsGet = true)]
    public string? Code { get; set; }

    [BindProperty]
    public string? SubmittedPassword { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        CurrentYear = DateTime.Now.Year;
        
        if (string.IsNullOrWhiteSpace(Code))
            return Page();

        var note = await db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);

        if (note == null)
        {
            AttemptedAndNotFound = true;
            return Page();
        }

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
            if (note.IsEncrypted)
                note.Content = encryptionService.Decrypt(note.Content);
            return Page();
        }

        if (note.IsEncrypted)
            note.Content = encryptionService.Decrypt(note.Content);

        Note = note;
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmAsync()
    {
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);
        if (note != null && note.DeleteAfterView && string.IsNullOrEmpty(note.Password))
        {
            note.Viewed = true;
            await db.SaveChangesAsync();
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

        var note = await db.Notes.FirstOrDefaultAsync(n => n.Code == Code && n.IsActive);
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
            await db.SaveChangesAsync();
            JustViewedDeletableNote = true;
        }

        if (note.IsEncrypted)
        {
            note.Content = encryptionService.Decrypt(note.Content);
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
            db.Notes.Remove(note);
            await db.SaveChangesAsync();
            return null;
        }

        return note;
    }
}