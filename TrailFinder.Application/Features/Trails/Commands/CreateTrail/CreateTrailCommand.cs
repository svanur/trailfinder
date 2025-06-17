using MediatR;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.CreateTrail;

public record CreateTrailCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal DistanceMeters { get; init; }
    public decimal ElevationGainMeters { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }
    public double StartPointLatitude { get; init; }
    public double StartPointLongitude { get; init; }
    public string? WebUrl { get; init; }
    public Guid? UserId { get; init; }

    public static CreateTrailCommand FromDto(CreateTrailDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        DistanceMeters = dto.DistanceMeters,
        ElevationGainMeters = dto.ElevationGainMeters,
        DifficultyLevel = dto.DifficultyLevel,
        StartPointLatitude = dto.StartPointLatitude,
        StartPointLongitude = dto.StartPointLongitude,
        WebUrl = dto.WebUrl,
        UserId = dto.UserId
    };
}