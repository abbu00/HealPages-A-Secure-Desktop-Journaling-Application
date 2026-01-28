using Microsoft.EntityFrameworkCore;


public class EntryMoodService
{
    private readonly Database _db;

    public EntryMoodService(Database dbContext)
    {
        _db = dbContext;
    }

    public async Task<List<EntryMood>> FetchAllEntryMoodsAsync()
    {
        return await _db.EntryMood.ToListAsync();
    }

    public async Task<List<EntryMood>> FetchEntryMoodsByUserIdAsync(int userId)
    {
        return await _db.EntryMood
            .Where(em => _db.Entry
                .Any(e => e.Id == em.EntryId && e.userID == userId))
            .ToListAsync();
    }

    public async Task<List<EntryMood>> LinkMoodsToEntryAsync(int entryId, List<int> moodIds, string relationType)
    {
        var linkedMoods = new List<EntryMood>();

        foreach (var moodId in moodIds)
        {
            var moodLink = new EntryMood
            {
                EntryId = entryId,
                MoodId = moodId,
                RelationshipType = relationType,
                CreatedAt = DateTime.UtcNow
            };

            _db.EntryMood.Add(moodLink);
            await _db.SaveChangesAsync();
            linkedMoods.Add(moodLink);
        }

        return linkedMoods;
    }

    public async Task<List<EntryMood>> LinkMoodsToUserEntryAsync(int entryId, int userId, List<int> moodIds, string relationType)
    {
        var entry = await _db.Entry
            .FirstOrDefaultAsync(e => e.Id == entryId && e.userID == userId);
        
        if (entry == null)
            return new List<EntryMood>();

        return await LinkMoodsToEntryAsync(entryId, moodIds, relationType);
    }

    public async Task<bool> DeleteAllMoodsForEntryAsync(int entryId)
    {
        var existingMoods = await _db.EntryMood
            .Where(em => em.EntryId == entryId)
            .ToListAsync();

        if (existingMoods.Any())
        {
            _db.EntryMood.RemoveRange(existingMoods);
            await _db.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> DeleteAllMoodsForUserEntryAsync(int entryId, int userId)
    {
        var entryExists = await _db.Entry
            .AnyAsync(e => e.Id == entryId && e.userID == userId);
        
        if (!entryExists)
            return false;

        return await DeleteAllMoodsForEntryAsync(entryId);
    }

    public async Task<List<EntryMood>> FetchMoodsForEntryAsync(int entryId)
    {
        return await _db.EntryMood
            .Where(em => em.EntryId == entryId)
            .ToListAsync();
    }

    // NEW: Fetch moods for entry with user validation
    public async Task<List<EntryMood>> FetchMoodsForUserEntryAsync(int entryId, int userId)
    {
        // Verify the entry belongs to the user first
        var entryExists = await _db.Entry
            .AnyAsync(e => e.Id == entryId && e.userID == userId);
        
        if (!entryExists)
            return new List<EntryMood>();

        return await FetchMoodsForEntryAsync(entryId);
    }
}