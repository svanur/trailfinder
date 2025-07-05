using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Location.Response;

namespace TrailFinder.Application.Features.Locations.GetLocations;

public record GetLocationsQuery : IRequest<PaginatedResult<LocationDto>>;