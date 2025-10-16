using System.ComponentModel.DataAnnotations;

public class Note
{
    public int Id { get; set; }
    [Required]
    public required string Content { get; set; }
    [Required]
    public required string Code { get; set; }
    public string? Password { get; set; }
    public bool DeleteAfterView { get; set; } = false;
    public DateTime? ExpiryDateUtc { get; set; }
    public bool Viewed { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
}