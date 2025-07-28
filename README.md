# Demo - .NET Graceful Shutdown


## Signals

- [SIGINT](https://en.wikipedia.org/wiki/Signal_(IPC)#SIGINT): When you press `Ctrl+C` in the terminal or execute the command `kill -2 <pid>`.
- [SIGTERM](https://en.wikipedia.org/wiki/Signal_(IPC)#SIGTERM): When the process is terminated by another process, such as `docker stop` or `kill -15 <pid>`.
- [SIGKILL](https://en.wikipedia.org/wiki/Signal_(IPC)#SIGKILL): This signal cannot be caught or ignored, and it immediately terminates the process. It is used as a last resort to kill a process that does not respond to other signals. Can simulate with `kill -9 <pid>`.


## Run the demos

From the root of the project, run the following command to build and run the demo:


### Demo 1 - Posix Signals

```bash
cd ./src/Demo.PosixSignals
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-posixsignals . && docker run --rm demo-posixsignals
```


### Demo 2 - IHostedLifecycleService

```bash
cd ./src/Demo.IHostedLifecycleService
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-ihostedlifecycleservice . && docker run --rm demo-ihostedlifecycleservice
```


### Demo 3 - IHostApplicationLifetime

```bash
cd ./src/Demo.IHostApplicationLifetime
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-ihostapplicationlifetime . && docker run --rm demo-ihostapplicationlifetime
```


### Demo 4 - Worker

```bash
cd ./src/Demo.Worker
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-worker . && docker run --rm demo-worker
```


### Demo 5 - IHostLifetime

```bash
cd ./src/Demo.IHostLifetime
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-ihostlifetime . && docker run --rm demo-ihostlifetime
```


### Demo 6 - Api

```bash
cd ./src/Demo.Api
```

**Command Line**
```bash
dotnet run
```

**Docker**
```bash
docker build -t demo-api . && docker run --rm demo-api
```



## References

* [Host shutdown](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=hostbuilder#host-shutdown)
* [Graceful shutdown of ASP.NET-based applications](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/graceful-shutdown/graceful-shutdown.md)
