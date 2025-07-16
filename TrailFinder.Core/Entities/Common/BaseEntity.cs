// TrailFinder.Core/Entities/Common/BaseEntity.cs
namespace TrailFinder.Core.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid? UserId { get; set; }
}