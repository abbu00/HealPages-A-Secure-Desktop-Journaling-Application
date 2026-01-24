using Microsoft.EntityFrameworkCore;
using HealPages.Data;
using HealPages.Models;
using HealPages.Services.Interfaces;

namespace HealPages.Services;

public class StreakService : IStreakService
{
    private readonly AppDbContext _db;

    public StreakService(AppDbContext db)
    {
        _db = db;
    }

    public async Task UpdateStreakAsync(DateOnly today)
    {
        var streak = await _db.Streaks.FirstOrDefaultAsync();

        if (streak == null)
        {
            streak = new Streak
            {
                CurrentStreak = 1,
                LongestStreak = 1,
                LastJournalDate = today
            };
            _db.Streaks.Add(streak);
        }
        else
        {
            if (streak.LastJournalDate == today.AddDays(-1))
                streak.CurrentStreak++;
            else
                streak.CurrentStreak = 1;

            streak.LongestStreak = Math.Max(
                streak.LongestStreak,
                streak.CurrentStreak);

            streak.LastJournalDate = today;
        }

        await _db.SaveChangesAsync();
    }
}
