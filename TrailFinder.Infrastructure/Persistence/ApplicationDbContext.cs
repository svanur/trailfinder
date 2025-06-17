using Microsoft.EntityFrameworkCore;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Entities;
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
    
        // Apply configuration
        modelBuilder.ApplyConfiguration(new TrailConfiguration());
    }
}