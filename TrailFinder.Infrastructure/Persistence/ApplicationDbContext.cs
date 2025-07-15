using Microsoft.EntityFrameworkCore;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.Configurations;

namespace TrailFinder.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext

{
    public DbSet<Trail> Trails => Set<Trail>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   // Check that the Supabase port is correcty, when debuggin glocally
        // Look at 'Supabase start' info:  DB URL: postgresql://postgres:postgres@127.0.0.1:54322/postgres
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=54322;Database=postgres;Username=postgres;Password=postgres",
            o => o.UseNetTopologySuite());
    }
    /*
        A typical hosted Supabase connection string will look something like this:
        Host=db.[YOUR_PROJECT_REF].supabase.co;Port=5432;Database=postgres;Username=postgres;Password=[YOUR_DB_PASSWORD];SSL Mode=Require;Trust Server Certificate=true 
     */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Register the enum type
        //modelBuilder.HasPostgresEnum<DifficultyLevel>("difficulty_level");
        modelBuilder.HasPostgresEnum<DifficultyLevel>();
        modelBuilder.HasPostgresEnum<RouteType>();
        modelBuilder.HasPostgresEnum<TerrainType>();

        modelBuilder.Entity<Trail>()
            .Property(t => t.RouteGeom)
            .HasColumnType("geometry(LineString, 4326)"); // Or whatever your actual type/SRID is

        /*
        var difficultyLevelConverter = new ValueConverter<DifficultyLevel, DifficultyLevel>(
            v => v,
            v => v
        );

        // And for nullable DifficultyLevel
        var nullableDifficultyLevelConverter = new ValueConverter<DifficultyLevel?, DifficultyLevel?>(
            v => v,
            v => v
        );
        
        // Apply the converters globally
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DifficultyLevel) || p.ClrType == typeof(DifficultyLevel?));

            foreach (var property in properties)
                property.SetValueConverter(
                    property.ClrType == typeof(DifficultyLevel)
                        ? difficultyLevelConverter
                        : nullableDifficultyLevelConverter
                );
        }
        */

        // Apply configuration
        modelBuilder.ApplyConfiguration(new TrailConfiguration());
    }
}