using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace SIGEDAA.Services;

public interface IAuditTrailService
{
    Task RecordAsync(string action, string detail, string? actor = null);
}

public sealed class AuditTrailService : IAuditTrailService
{
    private readonly string _logsDirectory;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public AuditTrailService(IWebHostEnvironment environment)
    {
        _logsDirectory = Path.Combine(environment.ContentRootPath, "Logs");
    }

    public async Task RecordAsync(string action, string detail, string? actor = null)
    {
        Directory.CreateDirectory(_logsDirectory);

        string actorValue = string.IsNullOrWhiteSpace(actor) ? "Sistema" : actor;
        string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{actorValue}\t{action}\t{detail}{Environment.NewLine}";
        string filePath = Path.Combine(_logsDirectory, $"audit-{DateTime.Now:yyyyMMdd}.log");

        await _lock.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(filePath, line, Encoding.UTF8);
        }
        finally
        {
            _lock.Release();
        }
    }
}
