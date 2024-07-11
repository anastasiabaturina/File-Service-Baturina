using FileService.Configuration;
using Microsoft.Extensions.Options;

namespace FileService.Services;

public class FileCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSettings _timeSettings;

    public FileCleanupService(IOptions<TimeSettings> timeSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _timeSettings = timeSettings.Value;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceFile = scope.ServiceProvider.GetRequiredService<IFileService>();
            await serviceFile.AutoDeleteFilesAsync(cancellationToken);

            await Task.Delay(TimeSpan.FromDays(_timeSettings.Day), cancellationToken);
        }
    }     
}