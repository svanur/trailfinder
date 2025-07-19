// TrailFinder.Infrastructure.Persistence.Configurations/GpxFilesConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class GpxFilesConfiguration : IEntityTypeConfiguration<GpxFile>
{
    public void Configure(EntityTypeBuilder<GpxFile> builder)
    {
        builder.ToTable("gpx_files", "public"); // Explicitly specify schema if not default public

        // Primary Key
        builder.HasKey(gf => gf.Id);
        builder.Property(gf => gf.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd(); // This matches DEFAULT gen_random_uuid()

        // Foreign Key to Trails (one-to-one relationship enforced by UNIQUE)
        builder.Property(gf => gf.TrailId)
            .HasColumnName("trail_id")
            .HasColumnType("uuid")
            .IsRequired(); // NOT NULL

        builder.HasIndex(gf => gf.TrailId)
            .IsUnique(); // UNIQUE constraint on trail_id

        // Relationship (Optional - EF Core can often infer from property names, but explicit is clear)
        // Adjust WithMany() / WithOne() based on your Trail entity's navigation properties
        builder.HasOne<Trail>() // GpxFile has one Trail
               .WithOne() // Trail has one GpxFile (or WithMany() if Trail can have many GpxFiles, but your schema says UNIQUE)
               .HasForeignKey<GpxFile>(gf => gf.TrailId) // Foreign key is TrailId on GpxFile
               .OnDelete(DeleteBehavior.Cascade); // Matches ON DELETE CASCADE

        // Storage and File Properties
        builder.Property(gf => gf.StoragePath)
            .HasColumnName("storage_path")
            .IsRequired() // NOT NULL
            .HasMaxLength(255); // Assuming a reasonable max length for paths

        // Add a unique index on storage_path as discussed for integrity
        builder.HasIndex(gf => gf.StoragePath)
            .IsUnique();

        builder.Property(gf => gf.OriginalFileName)
            .HasColumnName("original_file_name")
            .IsRequired() // NOT NULL
            .HasMaxLength(255); // VARCHAR(255)

        builder.Property(gf => gf.FileName)
            .HasColumnName("file_name")
            .IsRequired() // NOT NULL
            .HasMaxLength(255); // VARCHAR(255)

        builder.Property(gf => gf.FileSize)
            .HasColumnName("file_size")
            .HasColumnType("bigint") // BIGINT
            .IsRequired(); // NOT NULL

        builder.Property(gf => gf.ContentType)
            .HasColumnName("content_type")
            .IsRequired() // NOT NULL
            .HasMaxLength(100); // VARCHAR(100)

        // Soft Delete Flag
        builder.Property(gf => gf.IsActive)
            .HasColumnName("is_active")
            .IsRequired() // NOT NULL
            .HasDefaultValue(true); // DEFAULT TRUE

        // Audit Fields
        builder.Property(gf => gf.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone") // TIMESTAMPTZ
            .IsRequired() // NOT NULL
            .ValueGeneratedOnAdd(); // DEFAULT NOW()

        builder.Property(gf => gf.CreatedBy)
            .HasColumnName("created_by")
            .HasColumnType("uuid")
            .IsRequired(); // NOT NULL

        builder.Property(gf => gf.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone") // TIMESTAMPTZ
            .IsRequired(false); // Nullable, as it has no DEFAULT and is set by trigger

        builder.Property(gf => gf.UpdatedBy)
            .HasColumnName("updated_by")
            .HasColumnType("uuid")
            .IsRequired(false); // Nullable
    }
}