using Microsoft.EntityFrameworkCore;
using HealPages.Data;
using HealPages.Models;
using HealPages.Services.Interfaces;

namespace HealPages.Services;

public class JournalService : IJournalService
{
    private readonly AppDbContext _db;
    private readonly IStreakService _streakService;

    public JournalService(AppDbContext db, IStreakService streakService)
    {
        _db = db;
        _streakService = streakService;
    }

    public async Task<JournalEntry> CreateOrUpdateTodayAsync(JournalEntry entry)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var existing = await _db.JournalEntries
            .FirstOrDefaultAsync(j => j.Date == today);

        if (existing != null)
        {
            existing.Title = entry.Title;
            existing.Content = entry.Content;
            existing.PrimaryMood = entry.PrimaryMood;
            existing.SecondaryMoods = entry.SecondaryMoods;
            existing.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return existing;
        }

        entry.Date = today;
        entry.CreatedAt = DateTime.Now;
        entry.UpdatedAt = DateTime.Now;

        _db.JournalEntries.Add(entry);
        await _db.SaveChangesAsync();

        await _streakService.UpdateStreakAsync(today);

        return entry;
    }

    public async Task<List<JournalEntry>> GetAllAsync()
    {
        return await _db.JournalEntries
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }
}
