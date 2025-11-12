# Dockerfile for Render.com
# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container to /src
WORKDIR /src

# Copy the solution file to the /src directory
COPY TrailFinder.sln ./

# Copy all necessary project files from the repository root.
# This ensures all dependent projects are available in the build context.
COPY TrailFinder.Api/ TrailFinder.Api/
COPY TrailFinder.Application/ TrailFinder.Application/
COPY TrailFinder.Core/ TrailFinder.Core/
COPY TrailFinder.DB/ TrailFinder.DB/
COPY TrailFinder.Infrastructure/ TrailFinder.Infrastructure/
COPY TrailFinder.Contract/ TrailFinder.Contract/

# Restore and Publish the application
# We now reference the .csproj file by its full path within the WORKDIR (/src)
RUN dotnet publish "TrailFinder.Api/TrailFinder.Api.csproj" -c Release -o /app/publish

# Stage 2: Run the published application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080 
# Copy published output from the build stage
COPY --from=build /app/publish .

# Run the final application DLL
ENTRYPOINT ["dotnet", "TrailFinder.Api.dll"]