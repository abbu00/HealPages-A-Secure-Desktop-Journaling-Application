using Microsoft.EntityFrameworkCore;

public class EntryTagService
{
    private readonly Database _db;

    public EntryTagService(Database dbContext)
    {
        _db = dbContext;
    }

    // WARNING: This returns ALL entry tags for ALL users - use carefully!
    public async Task<List<EntryTag>> FetchAllEntryTagsAsync()
    {
        return await _db.EntryTag
            .Include(et => et.Tag)
            .ToListAsync();
    }

    // NEW: Get entry tags for a specific user
    public async Task<List<EntryTag>> FetchEntryTagsByUserIdAsync(int userId)
    {
        return await _db.EntryTag
            .Include(et => et.Tag)
            .Where(et => _db.Entry
                .Any(e => e.Id == et.entryId && e.userID == userId))
            .ToListAsync();
    }

    public async Task<List<EntryTag>> LinkTagsToEntryAsync(int entryId, List<int> tagIds)
    {
        var linkedTags = new List<EntryTag>();

        foreach (var tagId in tagIds)
        {
            var tagLink = new EntryTag
            {
                entryId = entryId,
                tagId = tagId,
                CreatedAt = DateTime.UtcNow
            };

            _db.EntryTag.Add(tagLink);
            await _db.SaveChangesAsync();

            linkedTags.Add(tagLink);
        }

        return linkedTags;
    }

    // NEW: Link tags with user validation
    public async Task<List<EntryTag>> LinkTagsToUserEntryAsync(int entryId, int userId, List<int> tagIds)
    {
        // First verify the entry belongs to the user
        var entry = await _db.Entry
            .FirstOrDefaultAsync(e => e.Id == entryId && e.userID == userId);
        
        if (entry == null)
            return new List<EntryTag>();

        return await LinkTagsToEntryAsync(entryId, tagIds);
    }

    public async Task<List<EntryTag>> FetchTagsForEntryAsync(int entryId)
    {
        return await _db.EntryTag
            .Where(et => et.entryId == entryId)
            .ToListAsync();
    }

    // NEW: Fetch tags for entry with user validation
    public async Task<List<EntryTag>> FetchTagsForUserEntryAsync(int entryId, int userId)
    {
        // Verify the entry belongs to the user first
        var entryExists = await _db.Entry
            .AnyAsync(e => e.Id == entryId && e.userID == userId);
        
        if (!entryExists)
            return new List<EntryTag>();

        return await FetchTagsForEntryAsync(entryId);
    }
    
    public async Task<bool> DeleteAllTagsForEntryAsync(int entryId)
    {
        var existingTags = await _db.EntryTag
            .Where(et => et.entryId == entryId)
            .ToListAsync();

        if (existingTags.Any())
        {
            _db.EntryTag.RemoveRange(existingTags);
            await _db.SaveChangesAsync();
        }

        return true;
    }

    // NEW: Delete tags for entry with user validation
    public async Task<bool> DeleteAllTagsForUserEntryAsync(int entryId, int userId)
    {
        // Verify the entry belongs to the user first
        var entryExists = await _db.Entry
            .AnyAsync(e => e.Id == entryId && e.userID == userId);
        
        if (!entryExists)
            return false;

        return await DeleteAllTagsForEntryAsync(entryId);
    }
}