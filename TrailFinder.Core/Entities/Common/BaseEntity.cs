// TrailFinder.Core/Entities/Common/BaseEntity.cs
namespace TrailFinder.Core.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    internal static string GenerateSlug(string name)
    {
        return name.ToLower()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace("/", "-");
    }
}