namespace TrailFinder.Core.DTOs.Location.Response;

public class LocationDto
{
    private LocationDto() { } // For EF Core

    public LocationDto(
        Guid id,
        string name,
        string slug,
        //Guid? parentId,
        LocationLiteDto? parentLocationDto,
        IEnumerable<LocationLiteDto>? childrenLocationsDto,
        string description,
        double latitude,
        double longitude,
        Guid userId
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        UserId = userId;

        //CreatedAt = DateTime.UtcNow;
        //UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public double Latitude { get; set; }

    public double Longitude { get; set; }
    //public LineString? PointGeom { get; set; }

    public LocationLiteDto? ParentLocationDto { get; set; }
    public IEnumerable<LocationLiteDto>? ChildrenLocationsDto { get; set; }

    public Guid UserId { get; set; }
}