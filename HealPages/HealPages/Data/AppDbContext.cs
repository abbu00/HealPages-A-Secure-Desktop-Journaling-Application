using Microsoft.EntityFrameworkCore;
using HealPages.Models;

namespace HealPages.Data;

public class AppDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Streak> Streaks => Set<Streak>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}
