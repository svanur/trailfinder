using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.Configurations; // Make sure this namespace is included

namespace TrailFinder.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext

{
    public DbSet<Trail> Trails { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<TrailLocation> TrailLocations { get; set; } = null!;
    public DbSet<Race> Races { get; set; } = null!;
    public DbSet<RaceTrail> RaceTrails { get; set; } = null!;
    public DbSet<RaceLocation> RaceLocations { get; set; } = null!;
    
    public DbSet<GpxFile> GpxFiles { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Register enum types
        modelBuilder.HasPostgresEnum<DifficultyLevel>();
        modelBuilder.HasPostgresEnum<LocationType>();
        modelBuilder.HasPostgresEnum<RaceStatus>();
        
        // Apply configuration for all entities
        modelBuilder.ApplyConfiguration(new TrailConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new TrailLocationConfiguration());
        modelBuilder.ApplyConfiguration(new RaceConfiguration());
        modelBuilder.ApplyConfiguration(new RaceTrailConfiguration());
        modelBuilder.ApplyConfiguration(new RaceLocationConfiguration());
    }
}