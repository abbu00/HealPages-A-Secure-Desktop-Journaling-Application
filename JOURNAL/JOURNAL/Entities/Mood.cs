using System.ComponentModel.DataAnnotations;

public class Mood
{
    [Key]
    public int Id { get; set; }
        
    public string Name { get; set; } = string.Empty;
        
    public string Category { get; set; } 
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation property
    public virtual ICollection<EntryMood> EntryMoods { get; set; } = new List<EntryMood>();

}