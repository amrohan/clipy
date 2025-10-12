using System.ComponentModel.DataAnnotations;

public class Note
{
    public int Id { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required string Code { get; set; }

    public bool DeleteAfterView { get; set; } = false;
    public bool Viewed { get; set; } = false;
    public bool IsActive { get; set; }
}
