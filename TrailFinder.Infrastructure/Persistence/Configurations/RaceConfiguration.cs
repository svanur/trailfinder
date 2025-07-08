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

        // Numeric properties
        builder.Property(r => r.RecurringMonth)
            .HasColumnName("recurring_month")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(r => r.RecurringMonth)
            .HasColumnName("recurring_week")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(r => r.RecurringWeekday)
            .HasColumnName("recurring_weekday")
            .HasColumnType("int")
            .IsRequired();

        // Boolean property
        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasColumnType("boolean")
            .IsRequired()
            .HasDefaultValue(true);

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

        //
        // Indices
        //
        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.UserId);

        //builder.HasIndex(t => t.StartPoint)
        //    .HasMethod("GIST");
    }
}