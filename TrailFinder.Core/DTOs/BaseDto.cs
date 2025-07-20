namespace TrailFinder.Core.DTOs;

public abstract class BaseDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    
    public DateTime? UpdatedAt { get; set; } // Nullable for initial creation
    public Guid? UpdatedBy { get; set; } // Nullable for initial creation
}