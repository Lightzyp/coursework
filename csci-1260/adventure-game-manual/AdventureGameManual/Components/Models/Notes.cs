namespace AdventureGameManual.Models;

public enum ReferenceKind { Character, Item }

public class NoteReference
{
    public ReferenceKind Kind { get; set; }
    public int TargetId { get; set; }
    public string? Label { get; set; }
}

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public List<NoteReference> References { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
