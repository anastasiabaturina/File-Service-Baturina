using FileService.Service;

namespace FileService;

public class AutoDeleteFile : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AutoDeleteFile(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async state =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceFile = scope.ServiceProvider.GetRequiredService<IServiceFile>();
            await serviceFile.AutoDeleteFile();
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_timer != null)
        {
            _timer.Change(Timeout.Infinite, 0);
            _timer = null;
        }
        return Task.CompletedTask;
    }
}
