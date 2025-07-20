namespace TrailFinder.Core.DTOs.Common;

public abstract class BaseDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    
    public DateTime? UpdatedAt { get; set; } // Nullable for initial creation
    public string? UpdatedBy { get; set; } // Nullable for initial creation
}