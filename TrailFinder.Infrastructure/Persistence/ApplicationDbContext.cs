using Microsoft.EntityFrameworkCore;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Trail> Trails => Set<Trail>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Trail>(entity =>
        {
            entity.ToTable("Trails");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(e => e.Slug)
                .IsUnique();

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.DistanceMeters)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(e => e.ElevationGainMeters)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(e => e.StartPoint)
                .IsRequired()
                .HasColumnType("geometry");

            entity.Property(e => e.RouteGeometry)
                .HasColumnType("geometry");

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.Property(e => e.UserId)
                .IsRequired();
        });
    }
}