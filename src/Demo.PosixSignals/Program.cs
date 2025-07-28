using System.Runtime.InteropServices;

var waitForExit = new ManualResetEventSlim(false);

PosixSignalRegistration.Create(PosixSignal.SIGINT, context =>
{
    Console.WriteLine("SIGINT received. Exiting...");
    waitForExit.Set();
});
PosixSignalRegistration.Create(PosixSignal.SIGTERM, context =>
{
    Console.WriteLine("SIGTERM received. Exiting...");
    waitForExit.Set();
});
PosixSignalRegistration.Create(PosixSignal.SIGQUIT, context =>
{
    Console.WriteLine("SIGQUIT received. Exiting...");
    waitForExit.Set();
});

Console.WriteLine("Application running. Send SIGINT, SIGTERM or SIGQUIT to exit.");

// Wait indefinitely until one of the signal handlers releases the wait
waitForExit.Wait();

Console.WriteLine("Application shutting down gracefully...");
