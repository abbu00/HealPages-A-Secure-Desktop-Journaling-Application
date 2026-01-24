namespace HealPages.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Mood PrimaryMood { get; set; }

    // CSV for simplicity: "Excited,Celebratory"
    public string SecondaryMoods { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
