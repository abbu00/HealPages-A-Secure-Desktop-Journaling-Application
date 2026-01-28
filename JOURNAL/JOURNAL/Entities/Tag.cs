using System.ComponentModel.DataAnnotations;

public class Tag
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<EntryTag> EntryTags { get; set; } = new List<EntryTag>();
}