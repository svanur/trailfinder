# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore (faster caching)
COPY . .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish TrailFinder.Api/TrailFinder.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Railway requires this)
EXPOSE 8080

# Use Railway's PORT env var (critical!)
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "TrailFinder.Api.dll"]