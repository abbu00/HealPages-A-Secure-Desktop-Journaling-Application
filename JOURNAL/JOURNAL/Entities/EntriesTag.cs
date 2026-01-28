using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class EntryTag
{
    [Key]
    public int entryTagId { get; set; }
        
    public int entryId { get; set; }
        
    [ForeignKey("entryId")]
    public Entry? Entry { get; set; }
        
    public int tagId { get; set; }
        
    [ForeignKey("TagId")]
    public Tag? Tag { get; set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}