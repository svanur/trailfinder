namespace TrailFinder.Core.DTOs.Location.Response;

public class LocationLiteDto
{
    private LocationLiteDto()
    {
    }

    public LocationLiteDto(
        Guid id,
        string name,
        string slug
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
    }

    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
}