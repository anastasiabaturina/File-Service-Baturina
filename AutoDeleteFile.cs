using FileService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService;

public class AutoDeleteFile : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _timeInterval;


    public AutoDeleteFile(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _timeInterval = configuration.GetValue<int>("Time:Hour");
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceFile = scope.ServiceProvider.GetRequiredService<IFileService>();
            await serviceFile.AutoDeleteFile();

            await Task.Delay(TimeSpan.FromHours(_timeInterval), stoppingToken);
        }
    }


}
