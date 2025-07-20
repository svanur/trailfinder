using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Persistence.Configurations;

public class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        builder.ToTable("races");

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

        // Required string properties
        builder.Property(r => r.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.Slug)
            .HasColumnName("slug")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .IsRequired();

        // enum property
        builder.Property(t => t.RaceStatus)
            .HasColumnName("race_status") 
            .HasColumnType("race_status") 
            .IsRequired();
        
        // Numeric properties
        builder.Property(r => r.RecurringMonth)
            .HasColumnName("recurring_month")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(r => r.RecurringWeek)
            .HasColumnName("recurring_week")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(r => r.RecurringWeekday)
            .HasColumnName("recurring_weekday")
            .HasColumnType("int")
            .IsRequired();
        
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

    }
}