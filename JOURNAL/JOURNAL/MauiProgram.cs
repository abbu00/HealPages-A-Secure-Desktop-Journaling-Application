using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JOURNAL;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");


        builder.Services.AddDbContext<Database>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });


#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif



        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<EntryService>();
        builder.Services.AddScoped<EntryTagService>();
        builder.Services.AddScoped<EntryMoodService>();
        builder.Services.AddScoped<MoodsService>();
        builder.Services.AddScoped<TagsService>();
        // Add this to your services
        builder.Services.AddScoped<UserSession>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Database>();
            db.Database.EnsureCreated();
            Console.WriteLine("[DB] Tables ensured");
        }

        return app;

    
}

}