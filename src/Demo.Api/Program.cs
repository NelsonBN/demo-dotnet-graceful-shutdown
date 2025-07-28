var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.UseHttpGracefulShutdown();

app.MapGet("/longrunning/{seconds}", async (
    ILogger<Program> logger,
    int seconds,
    CancellationToken cancellationToken) =>
{
    var now = DateTime.UtcNow;
    var endTime = now.AddSeconds(seconds);

    logger.LogInformation("Long running request started at {StartTime:HH:mm:ss.ffffff} and will end at {EndTime:HH:mm:ss.ffffff}", now, endTime);

    while(!cancellationToken.IsCancellationRequested && DateTime.UtcNow < endTime)
    {
        await Task.Delay(3_000, CancellationToken.None);
        logger.LogInformation(
            "Still running at {CurrentTime:HH:mm:ss.ffffff} - cancellationToken: {CancellationToken}",
            DateTime.UtcNow,
            cancellationToken.IsCancellationRequested);
    }

    logger.LogInformation("Long running request completed at {CompletionTime:HH:mm:ss.ffffff}", DateTime.UtcNow);

    return "Worked was done successfully";
});

await app.RunAsync();


public static class HttpGracefulShutdownMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpGracefulShutdown(this IApplicationBuilder builder)
        => builder.UseMiddleware<HttpGracefulShutdownMiddleware>();


    internal sealed class HttpGracefulShutdownMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;


        public async Task InvokeAsync(HttpContext context)
        {
            var hostApplicationLifetime = context.RequestServices.GetRequiredService<IHostApplicationLifetime>();
            var originalToken = context.RequestAborted;

            // Create a combined token
            using var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                originalToken,
                hostApplicationLifetime.ApplicationStopping);

            // Replace RequestAborted with the combined token
            context.RequestAborted = combinedTokenSource.Token;

            try
            {
                await _next(context);
            }
            catch(OperationCanceledException exception) when(combinedTokenSource.Token.IsCancellationRequested)
            {
                if(originalToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(
                        $"Request {context.TraceIdentifier}: Cancelled by client disconnect",
                        exception,
                        combinedTokenSource.Token);
                }

                if(hostApplicationLifetime.ApplicationStopping.IsCancellationRequested)
                {
                    throw new OperationCanceledException(
                        $"Request {context.TraceIdentifier}: Cancelled by application shutdown",
                        exception,
                        combinedTokenSource.Token);
                }

                throw; // Re-throw to maintain default behavior
            }
        }
    }
}
