using Microsoft.EntityFrameworkCore;

public class EntryService
{
    private readonly Database _db;

    public EntryService(Database dbContext)
    {
        _db = dbContext;
    }

    // Existing method - OK
    public async Task<Entry?> CreateEntryAsync(
        int userId,
        string heading,
        string content,
        int? mainMoodId = null,
        int? secondaryMoodId = null,
        List<int>? extraMoodIds = null,
        List<int>? associatedTagIds = null)
    {
        var newEntry = new Entry
        {
            userID = userId,
            title = heading,
            content = content,
            CreatedAt = DateTime.UtcNow
        };

        _db.Entry.Add(newEntry);
        await _db.SaveChangesAsync();

        var moodLinks = new List<EntryMood>();

        if (mainMoodId.HasValue)
        {
            moodLinks.Add(new EntryMood
            {
                EntryId = newEntry.Id,
                MoodId = mainMoodId.Value,
                RelationshipType = "primary",
                CreatedAt = DateTime.UtcNow
            });
        }

        if (secondaryMoodId.HasValue)
        {
            moodLinks.Add(new EntryMood
            {
                EntryId = newEntry.Id,
                MoodId = secondaryMoodId.Value,
                RelationshipType = "secondary",
                CreatedAt = DateTime.UtcNow
            });
        }

        if (extraMoodIds != null)
        {
            moodLinks.AddRange(
                extraMoodIds
                    .Distinct()
                    .Where(id => id != mainMoodId && id != secondaryMoodId)
                    .Select(id => new EntryMood
                    {
                        EntryId = newEntry.Id,
                        MoodId = id,
                        RelationshipType = "secondary",
                        CreatedAt = DateTime.UtcNow
                    })
            );
        }

        if (moodLinks.Any())
        {
            _db.EntryMood.AddRange(moodLinks);
        }

        if (associatedTagIds != null && associatedTagIds.Any())
        {
            var tagLinks = associatedTagIds.Select(id => new EntryTag
            {
                entryId = newEntry.Id,
                tagId = id,
                CreatedAt = DateTime.UtcNow
            });

            _db.EntryTag.AddRange(tagLinks);
        }

        await _db.SaveChangesAsync();
        return newEntry;
    }

    // FIXED: Add this method to get entries for specific user
    public async Task<List<Entry>> GetEntriesByUserIdAsync(int userId)
    {
        return await _db.Entry
            .AsNoTracking()
            .Where(e => e.userID == userId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

   
    // WARNING: This returns ALL entries for ALL users - use carefully!
    public async Task<List<Entry>> GetAllEntriesAsync()
    {
        return await _db.Entry
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    // FIXED: Add this method to get entry moods for specific user
    public async Task<List<EntryMood>> GetEntryMoodsByUserIdAsync(int userId)
    {
        return await _db.EntryMood
            .Where(em => _db.Entry.Any(e => e.Id == em.EntryId && e.userID == userId))
            .ToListAsync();
    }

    public async Task<List<EntryMood>> GetAllEntryMoodsAsync()
    {
        return await _db.EntryMood.ToListAsync();
    }

    // FIXED: Add this method to modify only user's own entries
    public async Task<bool> ModifyUserEntryAsync(
        int entryId,
        int userId, // Add user ID parameter for security
        string heading,
        string content,
        int? mainMoodId = null,
        int? secondaryMoodId = null,
        List<int>? extraMoodIds = null,
        List<int>? associatedTagIds = null)
    {
        // First check if this entry belongs to the user
        var entryToUpdate = await _db.Entry
            .FirstOrDefaultAsync(e => e.Id == entryId && e.userID == userId);
        
        if (entryToUpdate == null) return false;

        entryToUpdate.title = heading;
        entryToUpdate.content = content;
        entryToUpdate.CreatedAt = DateTime.UtcNow;

        var existingMoodLinks = _db.EntryMood.Where(em => em.EntryId == entryId);
        var existingTagLinks = _db.EntryTag.Where(et => et.entryId == entryId);

        _db.EntryMood.RemoveRange(existingMoodLinks);
        _db.EntryTag.RemoveRange(existingTagLinks);

        var moodLinks = new List<EntryMood>();

        if (mainMoodId.HasValue)
            moodLinks.Add(new EntryMood
            {
                EntryId = entryToUpdate.Id,
                MoodId = mainMoodId.Value,
                RelationshipType = "primary",
                CreatedAt = DateTime.UtcNow
            });

        if (secondaryMoodId.HasValue)
            moodLinks.Add(new EntryMood
            {
                EntryId = entryToUpdate.Id,
                MoodId = secondaryMoodId.Value,
                RelationshipType = "secondary",
                CreatedAt = DateTime.UtcNow
            });

        if (extraMoodIds != null)
        {
            moodLinks.AddRange(extraMoodIds.Select(id => new EntryMood
            {
                EntryId = entryToUpdate.Id,
                MoodId = id,
                RelationshipType = "additional",
                CreatedAt = DateTime.UtcNow
            }));
        }

        if (moodLinks.Any())
            _db.EntryMood.AddRange(moodLinks);

        if (associatedTagIds != null && associatedTagIds.Any())
        {
            var tagLinks = associatedTagIds.Select(id => new EntryTag
            {
                entryId = entryToUpdate.Id,
                tagId = id,
                CreatedAt = DateTime.UtcNow
            });

            _db.EntryTag.AddRange(tagLinks);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    // Keep existing ModifyEntryAsync for backward compatibility
    public async Task<bool> ModifyEntryAsync(
        int entryId,
        string heading,
        string content,
        int? mainMoodId = null,
        int? secondaryMoodId = null,
        List<int>? extraMoodIds = null,
        List<int>? associatedTagIds = null)
    {
        return await ModifyUserEntryAsync(entryId, -1, heading, content, 
            mainMoodId, secondaryMoodId, extraMoodIds, associatedTagIds);
    }
    
    
    public async Task<bool> DeleteUserEntryAsync(int entryId, int userId)
    {
        var entry = await _db.Entry.FirstOrDefaultAsync(e => e.Id == entryId && e.userID == userId);
        if (entry == null) return false;

        var moods = _db.EntryMood.Where(em => em.EntryId == entryId);
        var tags = _db.EntryTag.Where(et => et.entryId == entryId);

        _db.EntryMood.RemoveRange(moods);
        _db.EntryTag.RemoveRange(tags);
        _db.Entry.Remove(entry);

        await _db.SaveChangesAsync();
        return true;
    }

}