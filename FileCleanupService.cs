using FileService.Service;

namespace FileService;

public class FileCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _timeInterval;


    public FileCleanupService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _timeInterval = configuration.GetValue<int>("Time:Hour");
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceFile = scope.ServiceProvider.GetRequiredService<IFileService>();
            await serviceFile.AutoDeleteFilesAsync();

            await Task.Delay(TimeSpan.FromHours(_timeInterval), cancellationToken);
        }
    }
}