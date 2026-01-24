namespace HealPages.Services.Interfaces;

public interface IStreakService
{
    Task UpdateStreakAsync(DateOnly today);
}
