using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Enums;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class TrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("trails");
        
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");
        
        builder.Property(t => t.ParentId)
            .HasColumnName("parent_id")
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.Property(t => t.Name).HasColumnName("name");
        builder.Property(t => t.Slug).HasColumnName("slug");
        builder.Property(t => t.Description).HasColumnName("description");
        builder.Property(t => t.DistanceMeters).HasColumnName("distance_meters");
        builder.Property(t => t.ElevationGainMeters).HasColumnName("elevation_gain_meters");
        builder.Property(t => t.HasGpx).HasColumnName("has_gpx");
        builder.Property(t => t.RouteGeometry).HasColumnName("route_geom");
        builder.Property(t => t.WebUrl).HasColumnName("web_url");
        builder.Property(t => t.StartPoint).HasColumnName("start_point");
        builder.Property(t => t.RouteGeometry).HasColumnName("route_geom");  // Note: changed to match DB column name
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        
        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired(false);
        
        builder.Property(t => t.DifficultyLevelLevel)
            .HasColumnName("difficulty_level")
            .HasColumnType("difficulty_level")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<DifficultyLevel>(v, true)
            );

    }
}