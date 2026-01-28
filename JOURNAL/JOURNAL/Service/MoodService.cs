using Microsoft.EntityFrameworkCore;

public class MoodsService
{
    private readonly Database _context;

    public MoodsService(Database context)
    {
        _context = context;
    }

    public async Task<List<Mood>> GetAllMoodsAsync()
    {
        return await _context.Mood.ToListAsync();
    }



    // NEW: Get mood counts for a specific user
    public async Task<Dictionary<Mood, int>> GetMoodCountsByUserIdAsync(int userId)
    {
        var moodCounts = await _context.EntryMood
            .Where(em => _context.Entry
                .Any(e => e.Id == em.EntryId && e.userID == userId))
            .GroupBy(em => em.MoodId)
            .Select(g => new 
            { 
                MoodId = g.Key, 
                Count = g.Count() 
            })
            .ToListAsync();

        var moods = await _context.Mood
            .Where(m => moodCounts.Select(mc => mc.MoodId).Contains(m.Id))
            .ToListAsync();

        return moods.ToDictionary(
            mood => mood,
            mood => moodCounts.FirstOrDefault(mc => mc.MoodId == mood.Id)?.Count ?? 0
        );
    }

    
    
    

    
    
    // NEW: Get most used moods for a specific user
    public async Task<List<Mood>> GetTopMoodsByUserIdAsync(int userId, int count = 5)
    {
        var moodCounts = await GetMoodCountsByUserIdAsync(userId);
        return moodCounts
            .OrderByDescending(mc => mc.Value)
            .Take(count)
            .Select(mc => mc.Key)
            .ToList();
    }
    
    
    
    public async Task<List<Mood>> GetMoodsByUserIdAsync(int userId)
    {
        return await (
                from em in _context.EntryMood
                join e in _context.Entry on em.EntryId equals e.Id
                join m in _context.Mood on em.MoodId equals m.Id
                where e.userID == userId
                select m
            )
            .Distinct()
            .AsNoTracking()
            .ToListAsync();
    }

    
    
}


