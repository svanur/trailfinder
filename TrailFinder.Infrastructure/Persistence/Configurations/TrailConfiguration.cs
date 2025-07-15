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
            .IsRequired();
            
        // Numeric properties
        builder.Property(t => t.Distance)
            .HasColumnName("distance")
            .HasColumnType("decimal(10,2)")
            .IsRequired();
        
        builder.Property(t => t.ElevationGain)
            .HasColumnName("elevation_gain")
            .HasColumnType("double precision")  // PostgreSQL type for double
            .IsRequired();  // This is actually the default for non-nullable types
         
        builder.Property(t => t.DifficultyLevel)
            .HasColumnName("difficulty_level");
        
        /*
        builder.Property(t => t.DifficultyLevel)
            .HasColumnName("difficulty_level")
            .HasColumnType("difficulty_level")
            .HasConversion<string>();  // This tells EF Core to convert the enum to/from string
            */
        
        /*
        builder.Property(t => t.DifficultyLevel)
            .HasColumnType("difficulty_level")
            .HasConversion(
                v => v.HasValue ? v.Value.ToString().ToLower() : null,
                v => v == null ? null : (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), v, true)
            );
            */
        
        // Boolean property
        builder.Property(t => t.HasGpx)
            .HasColumnName("has_gpx")
            .HasColumnType("boolean")
            .IsRequired()
            .HasDefaultValue(false);
        
        // Geometry properties
        builder.Property(t => t.StartPoint)
            .HasColumnName("start_point")
            .HasColumnType("geometry(PointZ, 4326)")  // Changed from Point to PointZ
            .IsRequired(false);
    
        builder.Property(t => t.EndPoint)
            .HasColumnName("end_point")
            .HasColumnType("geometry(PointZ, 4326)")  // Changed from Point to PointZ
            .IsRequired(false);
    
        builder.Property(t => t.RouteGeom)
            .HasColumnName("route_geom")
            .HasColumnType("geometry(LineStringZ, 4326)")  // Changed from LineString to LineStringZ
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
/*
        builder.Property(t => t.DifficultyLevel)
            .HasColumnName("difficulty_level")
            .HasColumnType("difficulty_level");  // Specify the column type

        builder.Property(t => t.RouteType)
            .HasColumnName("route_type")
            .HasColumnType("route_type");        // Specify the column type

        builder.Property(t => t.TerrainType)
            .HasColumnName("terrain_type")
            .HasColumnType("terrain_type");      // Specify the column type
*/

        // Indexes
        builder.HasIndex(t => t.Slug)
            .IsUnique();
            
        
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.StartPoint)
            .HasMethod("GIST");
        builder.HasIndex(t => t.RouteGeom)
            .HasMethod("GIST");
        
    }
}
