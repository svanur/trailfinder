// TrailFinder.Core/Entities/Common/BaseEntity.cs
namespace TrailFinder.Core.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    public Guid? UserId { get; protected set; }
}