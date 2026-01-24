namespace HealPages.Models;

public class Streak
{
    public int Id { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly? LastJournalDate { get; set; }
}
