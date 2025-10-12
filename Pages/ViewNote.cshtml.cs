using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ViewNoteModel : PageModel
{
    private readonly AppDbContext _db;
    public ViewNoteModel(AppDbContext db) => _db = db;

    public Note? Note { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Code { get; set; }

    public bool Attempted { get; set; } = false;

    public bool WillDeleteAfterView { get; set; } = false;

    public void OnGet()
    {
        if (!string.IsNullOrWhiteSpace(Code))
        {
            Attempted = true;

            Note = _db.Notes.FirstOrDefault(n => n.Code == Code && n.IsActive);

            if (Note != null)
            {
                if (Note.DeleteAfterView && !Note.Viewed)
                {
                    WillDeleteAfterView = true;

                    Note.Viewed = true;
                    _db.SaveChanges();
                }
                else if (Note.DeleteAfterView && Note.Viewed)
                {
                    Note.IsActive = false;
                    _db.SaveChanges();
                    Note = null;
                }
            }
        }
    }
}
