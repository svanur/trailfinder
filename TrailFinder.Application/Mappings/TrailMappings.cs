using AutoMapper;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class TrailMappings : Profile
{
    // In use by AutoMapper
    public TrailMappings()
    {
        CreateMap<Trail, TrailListItemDto>()
            .ForMember(
                dest => dest.DistanceKm,
                opt =>
                    opt.MapFrom(src =>
                            Math.Round(src.DistanceMeters / 1000.0,
                                2) // Convert meters to KM for display, round to 2 decimals
                    )
            )
            .ForMember(dest => dest.StartGpxPoint, opt => opt.MapFrom(src =>
                    src.RouteGeom != null && src.RouteGeom.NumPoints > 0
                        ? new GpxPoint(src.RouteGeom.StartPoint)
                        : default // Or null, depending on if GpxPoint can be null
            ))
            .ForMember(dest => dest.EndGpxPoint, opt => opt.MapFrom(src =>
                    src.RouteGeom != null && src.RouteGeom.NumPoints > 0
                        ? new GpxPoint(src.RouteGeom.EndPoint)
                        : default // Or null
            ))

            // Ensure other mappings are present, including DistanceToUserMeters/Km if the repository adds it
            .ForMember(dest => dest.DistanceToUserMeters, opt => opt.Ignore()) // Calculated in handler/repo
            .ForMember(dest => dest.DistanceToUserKm, opt => opt.Ignore()) // Calculated in handler/repo
            .ForMember(
                dest => dest.TrailLocations,
                opt => opt.MapFrom(src => src.TrailLocations))
            ;

        // Mapping for detail view (includes full RouteGeom)
        CreateMap<Trail, TrailDetailDto>()
            // Copy all properties from Trail to TrailDetailDto
            //.IncludeBase<Trail, BaseDto>() // If TrailDetailDto inherits TrailListItemDto
            // Or map each property individually if not inheriting:
            // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            // ...
            
            // And explicitly map RouteGeom
            .ForMember(dest => dest.RouteGeom, opt => opt.MapFrom(src => src.RouteGeom))
            
            .ForMember(dest => dest.StartGpxPoint, opt => opt.MapFrom(src =>
                    src.RouteGeom != null && src.RouteGeom.NumPoints > 0
                        ? new GpxPoint(src.RouteGeom.StartPoint)
                        : default // Or null, depending on if GpxPoint can be null
            ))
            
            .ForMember(dest => dest.EndGpxPoint, opt => opt.MapFrom(src =>
                    src.RouteGeom != null && src.RouteGeom.NumPoints > 0
                        ? new GpxPoint(src.RouteGeom.EndPoint)
                        : default // Or null
            ))
            ;


        // List to PaginatedResult mapping
        /*
        CreateMap<PaginatedResult<Trail>, PaginatedResult<TrailDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
        */
    }
}