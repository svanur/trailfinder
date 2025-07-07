using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.Configurations;

namespace TrailFinder.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Trail> Trails => Set<Trail>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<TrailLocation> TrailLocations => Set<TrailLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Register the enum type
        //modelBuilder.HasPostgresEnum<DifficultyLevel>("difficulty_level");
        modelBuilder.HasPostgresEnum<DifficultyLevel>();
        modelBuilder.HasPostgresEnum<LocationType>();


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
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new TrailConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new TrailLocationConfiguration());
    }
}