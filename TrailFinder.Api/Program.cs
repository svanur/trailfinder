using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Supabase;
using TrailFinder.Api.Converters;
using TrailFinder.Application;
using TrailFinder.Core;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure;
using TrailFinder.Infrastructure.Configuration;
using TrailFinder.Infrastructure.Persistence;
using TrailFinder.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Npgsql", LogLevel.Information);

// Add services to the container
builder.Services
    .AddCore()
    .AddInfrastructure(builder.Configuration);

// Add controllers with all JSON options configured once
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.JsonSerializerOptions.Converters.Add(new LineStringConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();

// Add the configuration section
builder.Services.Configure<SupabaseSettings>(builder.Configuration.GetSection("Supabase"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSource = NpgsqlTrailFinderExtensions.CreateTrailFinderDataSource(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(dataSource,
            x => 
            {
                x.UseNetTopologySuite();
                x.EnableRetryOnFailure();
                x.MapEnum<DifficultyLevel>();
                x.MapEnum<RouteType>();
                x.MapEnum<TerrainType>();
            })
        .EnableSensitiveDataLogging()
        .LogTo(message =>
            {
                var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs", "ef-sql.log");
                File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
            },
            [DbLoggerCategory.Database.Command.Name],
            LogLevel.Information);
});

// Configure health checks

/*
builder.Services.AddHealthChecks()

    // Database health checks

    .AddDbContextCheck<ApplicationDbContext>("database")
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection"),  // Use "DefaultConnection" to match config
        name: "database",
        tags: new[] { "db", "postgresql" }
    )


    // Supabase health check
    .AddUrlGroup(
        new Uri(builder.Configuration["VITE_SUPABASE_URL"]!),
        name: "supabase-api",
        tags: ["external-service"])

    .AddTypeActivatedCheck<SupabaseStorageHealthCheck>(
        name: "supabase-storage", "storage", "supabase"
    )

    // Custom GPX directory access check
    .AddCheck("gpx-directory-access", () =>
        {
            try
            {
                var gpxPath = Path.Combine(builder.Environment.ContentRootPath, "storage", "gpx");

                // Check if the directory exists
                if (!Directory.Exists(gpxPath))
                {
                    return HealthCheckResult.Unhealthy($"GPX directory does not exist at: {gpxPath}");
                }

                // Test write permissions
                var testFile = Path.Combine(gpxPath, $"test_{Guid.NewGuid()}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);

                // Test read permissions by listing files
                _ = Directory.GetFiles(gpxPath);

                return HealthCheckResult.Healthy("GPX directory is accessible with read/write permissions");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("GPX directory access check failed", ex);
            }
        },
        tags: ["storage", "gpx"])

    // Disk storage health check
    .AddDiskStorageHealthCheck(options =>
    {
        options.AddDrive(@"C:\", 1024); // Checks if C: drive has at least 1GB free space
    }, "disk-space-check")


    // Memory health check
    .AddProcessAllocatedMemoryHealthCheck(
        maximumMegabytesAllocated: 1024, // 1GB maximum
        name: "memory",
        tags: ["resources"]);

*/

// Configure CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [])
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Register Supabase client
builder.Services.AddSingleton(provider =>
{
    var supabaseUrl = builder.Configuration["VITE_SUPABASE_URL"];
    var supabaseKey = builder.Configuration["VITE_SUPABASE_ANON_KEY"];

    if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
        throw new InvalidOperationException("Supabase configuration is missing");

    return new Client(supabaseUrl, supabaseKey);
});

var logsPath = Path.Combine(builder.Environment.ContentRootPath, "logs");
Directory.CreateDirectory(logsPath);

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the health check endpoint with detailed responses

/*
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false,
    Predicate = _ => true
});
*/

// Add specialized endpoints for different check types

/*
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = (check) => check.Tags.Contains("ready")
});
*/

/*
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true
});
*/

app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");
app.UseAuthorization();
app.MapControllers();
//app.MapHealthChecks("/health");

app.Run();