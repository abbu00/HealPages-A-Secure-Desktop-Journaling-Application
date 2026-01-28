using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PinCode { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Theme { get; set; } = "light";

    public DateTime createdAt { get; set; } = DateTime.UtcNow;
}