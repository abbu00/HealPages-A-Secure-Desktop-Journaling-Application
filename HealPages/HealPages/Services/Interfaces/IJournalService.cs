using HealPages.Models;

namespace HealPages.Services.Interfaces;

public interface IJournalService
{
    Task<JournalEntry> CreateOrUpdateTodayAsync(JournalEntry entry);
    Task<List<JournalEntry>> GetAllAsync();
}
