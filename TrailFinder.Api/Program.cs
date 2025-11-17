using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Supabase;
using TrailFinder.Api.Converters;
using TrailFinder.Api.Middleware;
using TrailFinder.Application;
using TrailFinder.Core;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure;
using TrailFinder.Infrastructure.Configuration;
using TrailFinder.Infrastructure.HealthChecks;
using TrailFinder.Infrastructure.Persistence;
using TrailFinder.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Npgsql", LogLevel.Information);

// Add services to the container
builder.Services
    .AddCore()
    .AddInfrastructure(builder.Configuration);

// Add versioning support
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        // The name of the format for API version groups.
        // 'v' for version, 'VVV' for the major, minor, and status.
        options.GroupNameFormat = "'v'VVV";

        // This option substitutes the version value in the URL.
        options.SubstituteApiVersionInUrl = true;
    });

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

                // Map the enums
                x.MapEnum<DifficultyLevel>();
                x.MapEnum<RouteType>();
                x.MapEnum<TerrainType>();
                x.MapEnum<LocationType>();
                x.MapEnum<SurfaceType>();
                x.MapEnum<RaceStatus>();
            })
        /*
            .EnableSensitiveDataLogging()
        .LogTo(message =>
            {
                var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs", "ef-sql.log");
                File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
            },
            [DbLoggerCategory.Database.Command.Name],
            LogLevel.Information)
        */
        ;
});

// Configure health checks
builder.Services.AddHealthChecks()

    // Database health checks
    .AddDbContextCheck<ApplicationDbContext>("database", tags: ["db", "postgresql", "ready"])


    // Supabase health check
    .AddUrlGroup(
        new Uri(new Uri(builder.Configuration.GetSection("Supabase")["Url"]!), "rest/v1/"), // Point to a known PostgREST endpoint
        name: "supabase-api",
        tags: ["external-service", "ready"])

    .AddTypeActivatedCheck<SupabaseStorageHealthCheck>(
        name: "supabase-storage",
        failureStatus: null, // Use null to default to Unhealthy on failure
        tags: ["storage", "supabase", "ready"],
        timeout: TimeSpan.FromSeconds(30) // Set a reasonable timeout, e.g., 30 seconds
    )

    // Configuration integrity check
    .AddCheck("configuration-check", () =>
        {
            var isSupabaseUrlConfigured = !string.IsNullOrEmpty(builder.Configuration.GetSection("Supabase")["Url"]);
            var isSupabaseKeyConfigured = !string.IsNullOrEmpty(builder.Configuration.GetSection("Supabase")["Key"]);
            var isApiKeyConfigured = !string.IsNullOrEmpty(builder.Configuration.GetSection("ApiSettings")["ApiKey"]);
            var isDefaultConnectionStringConfigured = !string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection"));

            if (isSupabaseUrlConfigured && isSupabaseKeyConfigured && isApiKeyConfigured && isDefaultConnectionStringConfigured)
            {
                return HealthCheckResult.Healthy("Critical configurations are present.");
            }
            else
            {
                var issues = new List<string>();
                if (!isSupabaseUrlConfigured) issues.Add("Supabase:Url");
                if (!isSupabaseKeyConfigured) issues.Add("Supabase:Key");
                if (!isApiKeyConfigured) issues.Add("ApiSettings:ApiKey");
                if (!isDefaultConnectionStringConfigured) issues.Add("ConnectionStrings:DefaultConnection");
                return HealthCheckResult.Unhealthy($"Missing critical configurations: {string.Join(", ", issues)}");
            }
        },
        tags: ["configuration", "ready"])

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

// Configure CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


// Configure Rate Limiting (built-in .NET 9)
builder.Services.AddRateLimiter(options =>
{
    var permitLimit = builder.Configuration.GetValue<int>("RateLimiting:PermitLimit");
    var windowInSeconds = builder.Configuration.GetValue<int>("RateLimiting:WindowInSeconds");

    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = permitLimit;
        opt.Window = TimeSpan.FromSeconds(windowInSeconds);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    // Rate limit by IP address
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            ipAddress,
            partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromSeconds(windowInSeconds),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.",
            token);
    };
});

// Register Supabase client
builder.Services.AddSingleton(provider =>
{
    var supabaseUrl = builder.Configuration.GetSection("Supabase")["Url"]; // searches appsettings.json
    var supabaseKey = builder.Configuration.GetSection("Supabase")["Key"];

    if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
    {
        throw new InvalidOperationException("Supabase configuration is missing");
    }

    return new Client(supabaseUrl, supabaseKey);
});

var logsPath = Path.Combine(builder.Environment.ContentRootPath, "logs");
Directory.CreateDirectory(logsPath);

/* railway*/
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");
/* railway*/

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the health check endpoint with detailed responses
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false,
    Predicate = _ => true
});

// Add specialized endpoints for different check types
// This endpoint will show the status of health checks tagged with "ready"
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = check => check.Tags.Contains("ready")
});

// This endpoint will show the status of all registered health checks, similar to /health,
// but typically used for liveness probes
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true
});

app.UseHttpsRedirection();

// Apply CORS before authentication
app.UseCors("AllowFrontend");

// Apply rate limiting
app.UseRateLimiter();

// Apply API Key authentication middleware
app.UseApiKeyAuthentication();

app.UseAuthorization();
app.MapControllers();
//app.MapHealthChecks("/health");

app.Run();