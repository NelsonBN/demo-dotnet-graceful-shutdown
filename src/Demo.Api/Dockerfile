FROM mcr.microsoft.com/dotnet/sdk:9.0.303 AS build-env

EXPOSE 8080
WORKDIR /src

COPY . ./

RUN dotnet restore ./*.csproj

RUN dotnet build ./*.csproj -c Release --no-restore
RUN dotnet publish ./*.csproj -c Release --no-build --no-restore -o /app



FROM mcr.microsoft.com/dotnet/aspnet:9.0.7 AS final-env

WORKDIR /app

COPY --from=build-env /app .

ENTRYPOINT ["dotnet", "Demo.Api.dll"]
