namespace TrailFinder.Core.DTOs.Location.Response;

public class LocationDto : BaseDto
{
    private LocationDto() { }


    public LocationDto(
        Guid id,
        Guid? parentId,
        string name,
        string slug,
        string description,
        double latitude,
        double longitude,

        LocationLiteDto? parentLocationDto,
        IEnumerable<LocationLiteDto>? childrenLocationsDto,

        Guid createdBy,
        DateTime createdAt,
        Guid? updatedBy,
        DateTime? updatedAt
    )
    {
        Id = id;
        ParentId = parentId;
        Name = name;
        Slug = slug;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        ParentLocationDto = parentLocationDto;
        ChildrenLocationsDto = childrenLocationsDto;

        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
    // For EF Core

    public Guid? ParentId { get; set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public double Latitude { get; set; }

    public double Longitude { get; set; }
    //public LineString? PointGeom { get; set; }

    public LocationLiteDto? ParentLocationDto { get; set; }
    public IEnumerable<LocationLiteDto>? ChildrenLocationsDto { get; set; }
}