using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

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
        
        builder.Property(t => t.ParentId)
            .HasColumnType("uuid")
            .HasColumnName("parent_id");

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
            .HasMaxLength(2000);

        // Numeric properties
        builder.Property(t => t.Latitude)
            .HasColumnName("latitude")
            .HasColumnType("double precision");

        builder.Property(t => t.Longitude)
            .HasColumnName("longitude")
            .HasColumnType("double precision");

        // Geometry properties
        /*
        builder.Property(t => t.PointGeom)
            .HasColumnName("point_geom")
            .HasColumnType("geometry(PointZ, 4326)")
            .IsRequired(false);
            */

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
    }
}