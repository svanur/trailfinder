using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class TrailLocationConfiguration : IEntityTypeConfiguration<TrailLocation>
{
    public void Configure(EntityTypeBuilder<TrailLocation> builder)
    {
        builder.ToTable("trail_locations");

        // Primary Key - It seems you intend 'Id' to be the primary key of this join table
        // AND you want the combination of TrailId and LocationId to be UNIQUE.
        builder.HasKey(t => t.Id);

        // Ensure the combination of TrailId and LocationId is unique
        builder.HasIndex(t => new { t.TrailId, t.LocationId }).IsUnique();

        // Assuming your DB schema is now: `id UUID PRIMARY KEY`, and `UNIQUE (trail_id, location_id)`
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd(); // Let DB generate ID if it's new

        builder.Property(t => t.TrailId)
            .HasColumnName("trail_id");
        
        builder.Property(t => t.LocationId)
            .HasColumnName("location_id");

        // THIS IS CRUCIAL: No HasColumnType for Enums in EF Core 5+
        // Npgsql's enum mapping handles the conversion.
        builder.Property(t => t.LocationType)
            .HasColumnName("location_type"); // Just specify the column name

        // Comment property
        builder.Property(t => t.Comment)
            .HasColumnName("comment")
            .IsRequired(false) // Allow nulls, matching DB schema
            .HasMaxLength(255); // Keep max length if desired

        // DisplayOrder property
        builder.Property(t => t.DisplayOrder)
            .HasColumnName("display_order")
            .HasColumnType("numeric"); // Map numeric to int? or decimal? based on usage
                                        // If it's always integers, `int?` in C# and `numeric` in DB is fine.
                                        // EF Core generally handles this, but explicitly setting `numeric` can help.

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

        // Add foreign key relationships (EF Core needs these for navigation properties)
        builder.HasOne(tl => tl.Trail)
            .WithMany(t => t.TrailLocations) //if Trail has a collection
            .HasForeignKey(tl => tl.TrailId);

        // Foreign key relationships
        builder.HasOne(tl => tl.Location) // TrailLocation has one Location
            .WithMany(l => l.TrailLocations) // Location has many TrailLocations
            .HasForeignKey(tl => tl.LocationId); // The foreign key property on TrailLocation
     
    }
}