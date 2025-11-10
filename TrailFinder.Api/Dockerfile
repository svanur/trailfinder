# Dockerfile for Render.com
# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container to /src or /app
WORKDIR /app

# Copy all project files from the Render build context (which is TrailFinder.API)
# The '.' here refers to the Render Root Directory (TrailFinder.API)
COPY . .

# Restore and Publish the application
# We reference the .csproj file directly from the current WORKDIR (/src)
RUN dotnet publish "TrailFinder.API.csproj" -c Release -o /app/publish

# Stage 2: Run the published application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080 
# Copy published output from the build stage
COPY --from=build /app/publish .

# Run the final application DLL
ENTRYPOINT ["dotnet", "TrailFinder.API.dll"]
