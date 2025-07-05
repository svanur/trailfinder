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
            .IsRequired();

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

        //
        // Indices
        //
        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.UserId);
    }
}