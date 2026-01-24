namespace HealPages.Services.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateAsync();
}
