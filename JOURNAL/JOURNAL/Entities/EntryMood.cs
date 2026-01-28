using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EntryMood
    {
        [Key]
        public int Id { get; set; }
        
        public int EntryId { get; set; }
        
        [ForeignKey("entryId")]
        public Entry? Entry { get; set; }
        
        public int MoodId { get; set; }
        
        [ForeignKey("MoodId")]
        public Mood? Mood { get; set; }
        
        public string RelationshipType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
