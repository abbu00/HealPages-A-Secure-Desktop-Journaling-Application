using Microsoft.EntityFrameworkCore;

public class Database : DbContext
{
    
    public string DbPath { get; }
    
    public DbSet<User> User => Set<User>();
    public DbSet<Mood> Mood => Set<Mood>();
    public DbSet<Tag> Tag => Set<Tag>();
    public DbSet<Entry> Entry => Set<Entry>();
    public DbSet<EntryMood> EntryMood => Set<EntryMood>();
    public DbSet<EntryTag> EntryTag => Set<EntryTag>();
    

    public Database(DbContextOptions<Database> options)
        : base(options)
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Combine(folder, "app.db");

        System.Diagnostics.Debug.WriteLine($"[DB PATH] {DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.PinCode).IsRequired();
            entity.Property(e => e.Theme).HasDefaultValue("light");
            entity.Property(e => e.createdAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<Mood>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.createdAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<EntryMood>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<EntryTag>(entity =>
        {
            entity.HasKey(e => e.entryTagId);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });
    }
}
