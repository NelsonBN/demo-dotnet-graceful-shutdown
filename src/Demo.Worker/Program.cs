var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(15));
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

await app.RunAsync();


public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][EXECUTE] Running...", DateTimeOffset.UtcNow);
            await Task.Delay(5_000, CancellationToken.None);
        }

        logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][EXECUTE][IS CANCELLATION REQUESTED]", DateTimeOffset.UtcNow);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][STAR] Called", DateTimeOffset.UtcNow);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][STOP] Called", DateTimeOffset.UtcNow);
        await base.StopAsync(cancellationToken);
    }
}
