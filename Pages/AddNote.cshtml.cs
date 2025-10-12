using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AddNoteModel : PageModel
{
    private readonly AppDbContext _db;

    public AddNoteModel(AppDbContext db) => _db = db;

    [BindProperty]
    [Required(ErrorMessage = "Note content is required.")]
    public required string NoteContent { get; set; }

    [BindProperty]
    public string? Code { get; set; }

    [BindProperty]
    public bool DeleteAfterView { get; set; } = false;

    public string? NoteUrl { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var code = string.IsNullOrWhiteSpace(Code)
            ? Guid.NewGuid().ToString("n").Substring(0, 6)
            : Code;

        var exists = await _db.Notes.AnyAsync(n => n.Code == code);
        if (exists)
        {
            ErrorMessage = "This code already exists. Please choose a different one.";
            return Page();
        }

        var note = new Note
        {
            Content = NoteContent,
            Code = code,
            DeleteAfterView = DeleteAfterView,
            Viewed = false,
            IsActive = true
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        NoteUrl = Url.Page("/ViewNote", null, new { code = code }, Request.Scheme);
        ModelState.Clear();

        return Page();
    }
}
