using MediatR;
using TrailFinder.Core.DTOs.Location.Response;

namespace TrailFinder.Application.Features.Locations.GetLocation;

public record GetLocationQuery(Guid Id) : IRequest<LocationDto>;