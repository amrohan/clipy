using System.ComponentModel.DataAnnotations;
using clipy.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace clipy.Pages;

public class EditNoteModel(
    AppDbContext db,
    IEncryptionService encryptionService,
    IFileStorageService storage,
    UserManager<IdentityUser> userManager)
    : PageModel
{
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [BindProperty] public int Id { get; set; }

    [BindProperty, Required] public string NoteContent { get; set; } = string.Empty;

    [BindProperty] public string? Password { get; set; }

    [BindProperty] public bool DeleteAfterView { get; set; }

    [BindProperty] public string ExpiryOption { get; set; } = "never";

    [BindProperty] public DateTime? CustomExpiryDate { get; set; }

    [BindProperty] public IFormFile? UploadFile { get; set; }

    public string? ExistingFileName { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = _userManager.GetUserId(User);

        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id);

        if (note == null || note.UserId != userId)
            return Forbid();

        Id = note.Id;
        NoteContent = note.IsEncrypted
            ? encryptionService.Decrypt(note.Content)
            : note.Content;

        DeleteAfterView = note.DeleteAfterView;
        ExistingFileName = note.OriginalFileName;

        if (note.ExpiryDateUtc.HasValue)
        {
            CustomExpiryDate = note.ExpiryDateUtc.Value;
            ExpiryOption = "custom";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = _userManager.GetUserId(User);

        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == Id);

        if (note == null || note.UserId != userId)
            return Forbid();

        if (!ModelState.IsValid)
            return Page();

        // Encrypt again
        note.Content = encryptionService.Encrypt(NoteContent);

        // Password update (optional)
        if (!string.IsNullOrWhiteSpace(Password))
        {
            note.Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        note.DeleteAfterView = DeleteAfterView;

        // Expiry
        DateTime? expiryDateUtc = null;

        switch (ExpiryOption)
        {
            case "1h": expiryDateUtc = DateTime.UtcNow.AddHours(1); break;
            case "24h": expiryDateUtc = DateTime.UtcNow.AddDays(1); break;
            case "7d": expiryDateUtc = DateTime.UtcNow.AddDays(7); break;
            case "custom":
                if (CustomExpiryDate.HasValue)
                {
                    var d = CustomExpiryDate.Value.Date;
                    expiryDateUtc = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59, DateTimeKind.Utc);
                }

                break;
        }

        note.ExpiryDateUtc = expiryDateUtc;

        // File update
        if (UploadFile != null && UploadFile.Length > 0)
        {
            var storedFileName = $"{Guid.NewGuid()}_{UploadFile.FileName}";
            await storage.UploadAsync(UploadFile, storedFileName);

            note.FileName = storedFileName;
            note.OriginalFileName = UploadFile.FileName;
        }

        await db.SaveChangesAsync();

        return RedirectToPage("/MyNotes");
    }
}