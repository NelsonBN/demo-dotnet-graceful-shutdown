var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
await app.RunAsync();


public class Worker(ILogger<Worker> logger) : BackgroundService, IHostedLifecycleService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);
            await Task.Delay(500, stoppingToken);
        }
        logger.LogInformation("Worker stopped running at: {time}", DateTimeOffset.UtcNow);
    }

    public async Task StartingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartingAsync called - Before delay");
        await Task.Delay(3_000, cancellationToken);
        logger.LogInformation("StartingAsync called - After delay");
    }
    public Task StartedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartedAsync called");
        return Task.CompletedTask;
    }
    public async Task StoppingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StoppingAsync called - Before delay");
        await Task.Delay(3_000, cancellationToken);
        logger.LogInformation("StoppingAsync called - After delay");
    }
    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StoppedAsync called");
        return Task.CompletedTask;
    }
}
