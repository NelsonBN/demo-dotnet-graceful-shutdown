using System.Runtime.InteropServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IHostLifetime, GracefulShutdown>();
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
            await Task.Delay(15_000, CancellationToken.None);
        }

        _logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][EXECUTE][IS CANCELLATION REQUESTED]", DateTimeOffset.UtcNow);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{time:HH:mm:ss.fffff}][WORKER][STOP] Called", DateTimeOffset.UtcNow);

        return base.StopAsync(cancellationToken);
    }
}

public class GracefulShutdown(
    ILogger<GracefulShutdown> logger,
    IHostApplicationLifetime applicationLifetime) : IHostLifetime, IDisposable
{
    private readonly ILogger<GracefulShutdown> _logger = logger;
    private readonly IHostApplicationLifetime _applicationLifetime = applicationLifetime;
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(3);
    private IEnumerable<IDisposable>? _disposables;

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        _disposables =
        [
            PosixSignalRegistration.Create(PosixSignal.SIGINT,  _handleSignal),
            PosixSignalRegistration.Create(PosixSignal.SIGQUIT, _handleSignal),
            PosixSignalRegistration.Create(PosixSignal.SIGTERM, _handleSignal)
        ];
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GRACEFUL SHUTDOWN] StopAsync called - cleaning up resources");
        return Task.CompletedTask;
    }

    private void _handleSignal(PosixSignalContext context)
    {
        context.Cancel = true;
        _logger.LogInformation("[{time:HH:mm:ss.fffff}][GRACEFUL SHUTDOWN] Received signal {signal}, shutting down in {delay} seconds", DateTimeOffset.UtcNow, context.Signal, _delay.TotalSeconds);

        Task.Delay(_delay).ContinueWith(_ =>
        {
            _logger.LogInformation("[{time:HH:mm:ss.fffff}][GRACEFUL SHUTDOWN] Delayed shutdown complete, stopping application.", DateTimeOffset.UtcNow);
            _applicationLifetime.StopApplication();
        });
    }

    public void Dispose()
    {
        foreach(var disposable in _disposables ?? Enumerable.Empty<IDisposable>())
        {
            disposable.Dispose();
        }
    }
}
