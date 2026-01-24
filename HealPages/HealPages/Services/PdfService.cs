using HealPages.Services.Interfaces;

namespace HealPages.Services;

public class PdfService : IPdfService
{
    public Task<byte[]> GenerateAsync()
    {
        // Hook QuestPDF / iText here later
        return Task.FromResult(Array.Empty<byte>());
    }
}