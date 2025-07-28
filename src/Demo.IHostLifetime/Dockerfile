FROM mcr.microsoft.com/dotnet/sdk:9.0.303 AS build-env

WORKDIR /src

COPY . ./

RUN dotnet restore ./*.csproj

RUN dotnet build ./*.csproj \
    -c Release \
    --no-restore \
    -o /out



FROM mcr.microsoft.com/dotnet/runtime:9.0.7

WORKDIR /app
COPY --from=build-env /out .

USER app

ENTRYPOINT ["dotnet", "Demo.IHostLifetime.dll"]
