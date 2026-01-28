using Microsoft.EntityFrameworkCore;

public class TagsService
{
    private readonly Database _context;

    public TagsService(Database context)
    {
        _context = context;
    }

    // Get all tags from the database
    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tag.ToListAsync();
    }

    // NEW: Get tags for a specific user's entries
    public async Task<List<Tag>> GetTagsByUserIdAsync(int userId)
    {
        // Get tags that are associated with entries belonging to the user
        return await _context.Tag
            .Where(t => _context.EntryTag
                .Any(et => et.tagId == t.Id && 
                      _context.Entry
                          .Any(e => e.Id == et.entryId && e.userID == userId)))
            .Distinct()
            .ToListAsync();
    }
    
    
    
    public async Task<Dictionary<Tag, int>> GetTagCountsByUserIdAsync(int userId)
    {
        var query = from et in _context.EntryTag
            join e in _context.Entry on et.entryId equals e.Id
            join t in _context.Tag on et.tagId equals t.Id
            where e.userID == userId
            group t by t into g
            select new
            {
                Tag = g.Key,
                Count = g.Count()
            };

        var result = await query.ToListAsync();
        return result.ToDictionary(x => x.Tag, x => x.Count);
    }


   
    // NEW: Get most used tags for a specific user
    public async Task<List<Tag>> GetTopTagsByUserIdAsync(int userId, int count = 5)
    {
        var tagCounts = await GetTagCountsByUserIdAsync(userId);
        return tagCounts
            .OrderByDescending(tc => tc.Value)
            .Take(count)
            .Select(tc => tc.Key)
            .ToList();
    }
}