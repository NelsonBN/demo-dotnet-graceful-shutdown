var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<TimerService>();

var app = builder.Build();

var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
appLifetime.ApplicationStarted.Register(() => Console.WriteLine("Application has started"));
appLifetime.ApplicationStopping.Register(() => Console.WriteLine("Application is stopping"));
appLifetime.ApplicationStopped.Register(() => Console.WriteLine("Application has stopped"));

await app.RunAsync();


public class TimerService(
    ILogger<TimerService> logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly ILogger<TimerService> _logger = logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TimerService is starting");
        await Task.Delay(2000, stoppingToken);
        _logger.LogInformation("TimerService is stopping");
        _hostApplicationLifetime.StopApplication();
    }
}
