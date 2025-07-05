namespace TrailFinder.Core.DTOs.Location.Response;

public class LocationDto
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    //public LineString? PointGeom { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    
    public LocationDto()
    {
    }

    public LocationDto(
        Guid id,
        Guid? parentId,
        string name,
        string slug,
        string description,
        double latitude,
        double longitude,
        DateTime createdAt,
        DateTime updatedAt,
        Guid userId
    )
    {
        Id = id;
        ParentId = parentId;
        Name = name;
        Slug = slug;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

}