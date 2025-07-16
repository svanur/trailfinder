using MediatR;
using TrailFinder.Core.DTOs.Location.Response;

namespace TrailFinder.Application.Features.Locations.GetLocationBySlug;

public record GetLocationBySlugQuery(string Slug) : IRequest<LocationDto?>;