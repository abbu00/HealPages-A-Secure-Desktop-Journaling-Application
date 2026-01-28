using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Entry { 
    [Key]
    public int Id { get; set; }
    public int userID { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public string title { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


