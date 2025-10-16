using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AddNoteModel : PageModel
{
    private readonly AppDbContext _db;
    public AddNoteModel(AppDbContext db) => _db = db;

    [BindProperty, Required(ErrorMessage = "Note content is required.")]
    public required string NoteContent { get; set; }

    [BindProperty]
    public string? Code { get; set; }

    [BindProperty]
    public bool DeleteAfterView { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string ExpiryOption { get; set; } = "never";


    [BindProperty]
    public DateTime? CustomExpiryDate { get; set; }

    public string? NoteUrl { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {

        if (ExpiryOption == "custom" && (!CustomExpiryDate.HasValue || CustomExpiryDate.Value < DateTime.Today))
        {
            ModelState.AddModelError(nameof(CustomExpiryDate), "Please select a valid future date for custom expiration.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var code = string.IsNullOrWhiteSpace(Code)
            ? Guid.NewGuid().ToString("n").Substring(0, 6)
            : Code;

        if (await _db.Notes.AnyAsync(n => n.Code == code))
        {
            ErrorMessage = "This code is already in use. Please choose a different one.";
            return Page();
        }

        string? hashedPassword = !string.IsNullOrWhiteSpace(Password)
            ? BCrypt.Net.BCrypt.HashPassword(Password)
            : null;


        DateTime? expiryDateUtc = null;
        switch (ExpiryOption)
        {
            case "1h": expiryDateUtc = DateTime.UtcNow.AddHours(1); break;
            case "24h": expiryDateUtc = DateTime.UtcNow.AddDays(1); break;
            case "7d": expiryDateUtc = DateTime.UtcNow.AddDays(7); break;
            case "custom":
                if (CustomExpiryDate.HasValue)
                {

                    var userDate = CustomExpiryDate.Value.Date;
                    expiryDateUtc = new DateTime(userDate.Year, userDate.Month, userDate.Day, 23, 59, 59, DateTimeKind.Utc);
                }
                break;
        }

        var note = new Note
        {
            Content = NoteContent,
            Code = code,
            Password = hashedPassword,
            DeleteAfterView = DeleteAfterView,
            ExpiryDateUtc = expiryDateUtc
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        NoteUrl = Url.Page("/ViewNote", null, new { code = code }, Request.Scheme);
        ModelState.Clear();

        return Page();
    }
}