using Npgsql;
using TrailFinder.Core.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add Npgsql enum mappings BEFORE registering DbContext
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder
    .MapEnum<DifficultyLevel>("difficulty_level")
    .MapEnum<RouteType>("route_type")
    .MapEnum<TerrainType>("terrain_type");
    
var dataSource = dataSourceBuilder.Build();

// Register the data source for dependency injection
builder.Services.AddSingleton(dataSource);


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
        // Handle potential NaN values:
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Remove the separate AddControllers() call

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();