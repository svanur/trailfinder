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
        
        // Primary Key
        builder.HasKey(t => t.Id);
        
        // Base properties
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();
        
        builder.Property(t => t.ParentId)
            .HasColumnName("parent_id")
            .HasColumnType("uuid")
            .IsRequired(false);
        
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
            .IsRequired();
            
        // Numeric properties
        builder.Property(t => t.DistanceMeters)
            .HasColumnName("distance_meters")
            .HasColumnType("decimal(10,2)")
            .IsRequired();
            
        builder.Property(t => t.ElevationGainMeters)
            .HasColumnName("elevation_gain_meters")
            .HasColumnType("double precision")  // PostgreSQL type for double
            .IsRequired();  // This is actually the default for non-nullable types

            
        // Boolean property
        builder.Property(t => t.HasGpx)
            .HasColumnName("has_gpx")
            .HasColumnType("boolean")
            .IsRequired()
            .HasDefaultValue(false);
            
        // Optional string property
        builder.Property(t => t.WebUrl)
            .HasColumnName("web_url")
            .HasMaxLength(2048)
            .IsRequired(false);
        
        // Geometry properties
        builder.Property(t => t.StartPoint)
            .HasColumnName("start_point")
            .HasColumnType("geometry(Point, 4326)")
            .IsRequired(false);
            
        builder.Property(t => t.EndPoint)
            .HasColumnName("end_point")
            .HasColumnType("geometry(Point, 4326)")
            .IsRequired(false);
            
        builder.Property(t => t.RouteGeom)
            .HasColumnName("route_geom")
            .HasColumnType("geometry(LineString, 4326)")
            .IsRequired(false);

        // Timestamps
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .ValueGeneratedOnAdd();
            
        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .ValueGeneratedOnUpdate();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.Property(t => t.DifficultyLevel)
            .HasColumnName("difficulty_level")
            .HasColumnType("difficulty_level")
            .IsRequired(false)
            .HasConversion(
                v => v.ToString()!.ToLowerInvariant(),
                v => Enum.Parse<DifficultyLevel>(v, true)
            );

        // Indexes
        builder.HasIndex(t => t.Slug)
            .IsUnique();
            
        builder.HasIndex(t => t.ParentId);
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.StartPoint)
            .HasMethod("GIST");
        builder.HasIndex(t => t.RouteGeom)
            .HasMethod("GIST");
            
        // Relationships
        builder.HasOne<Trail>()
            .WithMany()
            .HasForeignKey(t => t.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
