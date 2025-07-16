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
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .ValueGeneratedOnUpdate();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid");

        //
        // Indices
        //
        builder.HasIndex(t => t.UserId);
    }
}