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
        // If you intended (TrailId, LocationId) to be the *PRIMARY KEY*
        // then remove builder.HasKey(t => t.Id); and use builder.HasKey(t => new { t.TrailId, t.Id });
        // But your current DB schema for trail_locations states primary key (trail_id, location_id)
        // If your DB schema is `PRIMARY KEY (trail_id, location_id)` and not `PRIMARY KEY (id)`
        // THEN YOU NEED TO CHANGE YOUR HasKey here to:
        // builder.HasKey(t => new { t.TrailId, t.LocationId });
        // And remove `builder.Property(t => t.Id)` entirely, or make Id a simple unique identifier
        // for internal app use without mapping it as a PK.
        // Given your current database schema and the error, it seems `(trail_id, location_id)` is the PK.
        // I will assume for now your DB has `PRIMARY KEY (trail_id, location_id)` and NO `id` column.
        // If your database *does* have an `id` column as PK, then your `TrailLocation` entity needs to reflect that.
        // Let's go with your originally provided DB schema which did NOT have an `id` column initially.

        // Re-reading your latest DB schema, you *did* add `id` but kept `(trail_id, location_id)` as PK.
        // This is the problematic definition we discussed. Let's assume you've *corrected* the DB to:
        // create table public.trail_locations (id UUID PRIMARY KEY, trail_id UUID NOT NULL UNIQUE, location_id UUID NOT NULL UNIQUE, etc.)
        // OR primary key (id), UNIQUE (trail_id, location_id) as I recommended.
        // If you haven't, that's your root DB schema problem.

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

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired(false);

        // Add foreign key relationships (EF Core needs these for navigation properties)
        builder.HasOne(tl => tl.Trail)
               .WithMany() // Or .WithMany(t => t.TrailLocations) if Trail has a collection
               .HasForeignKey(tl => tl.TrailId);

        builder.HasOne(tl => tl.Location)
               .WithMany() // Or .WithMany(l => l.TrailLocations) if Location has a collection
               .HasForeignKey(tl => tl.LocationId);
    }
}