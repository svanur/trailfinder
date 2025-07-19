using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;
using TrailFinder.Infrastructure.Persistence.Converters;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class TrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("trails");

        //
        // Primary Key
        //
        builder.HasKey(t => t.Id);

        //
        // Base properties
        //
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        // Required string properties
        builder.Property(t => t.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Slug)
            .HasColumnName("slug")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(2000);

        // Numeric properties
        builder.Property(t => t.DistanceMeters)
            .HasColumnName("distance_meters")
            .HasConversion<DoubleToIntConverter>()
            .IsRequired();

        builder.Property(t => t.ElevationGainMeters)
            .HasColumnName("elevation_gain_meters")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.DifficultyLevel)
            .HasColumnName("difficulty_level");
        
        builder.Property(t => t.RouteType)
            .HasColumnName("route_type");
        
        builder.Property(t => t.TerrainType)
            .HasColumnName("terrain_type");
        
        builder.Property(t => t.SurfaceType)
            .HasColumnName("surface_type");

        builder.Property(t => t.RouteGeom)
            .HasColumnName("route_geom")
            .HasColumnType("geometry(LineStringZ, 4326)") // Changed from LineString to LineStringZ
            .IsRequired(false);

        // Optional string property
        builder.Property(t => t.WebUrl)
            .HasColumnName("web_url")
            .HasMaxLength(2048)
            .IsRequired(false);

        // Timestamps
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .ValueGeneratedOnAdd(); // Only set ValueGeneratedOnAdd if DB defaults it

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .ValueGeneratedOnUpdate(); // Only set ValueGeneratedOnUpdate if DB defaults it

        // User IDs
        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by")
            .HasColumnType("uuid");
        
        builder.Property(t => t.UpdatedBy)
            .HasColumnName("updated_by")
            .HasColumnType("uuid")
            .IsRequired(false);

        //
        // Indices
        //
        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.CreatedBy);

        builder.HasIndex(t => t.RouteGeom)
            .HasMethod("GIST");
    }
}