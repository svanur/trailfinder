using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class RaceTrailConfiguration : IEntityTypeConfiguration<RaceTrail>
{
    public void Configure(EntityTypeBuilder<RaceTrail> builder)
    {
        builder.ToTable("race_trails");

        //
        // Primary Key
        //
        builder.HasKey(r => r.Id);

        //
        // Base properties
        //
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        
        builder.Property(r => r.RaceId)
            .HasColumnName("race_id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();
        
        
        builder.Property(r => r.TrailId)
            .HasColumnName("trail_id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();
        
        
        // Required string properties
        builder.Property(r => r.Comment)
            .HasColumnName("comment")
            .IsRequired(false)
            .HasMaxLength(255);

        // enum property
        builder.Property(t => t.RaceStatus)
            .HasColumnName("race_status") 
            .HasColumnType("race_status") 
            .IsRequired();
        
        // Numeric properties
        builder.Property(r => r.DisplayOrder)
            .HasColumnName("display_order")
            .HasColumnType("int")
            .IsRequired();
        
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
        builder.HasIndex(t => t.CreatedBy);
    }
}