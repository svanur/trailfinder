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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL enums
        //modelBuilder.HasPostgresEnum<DifficultyLevel>("difficulty_level");
        //modelBuilder.HasPostgresEnum<RouteType>("route_type");
        //modelBuilder.HasPostgresEnum<TerrainType>("terrain_type");

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new TrailConfiguration());
    }
}