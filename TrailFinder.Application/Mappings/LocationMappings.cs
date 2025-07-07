using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class LocationMappings : Profile
{
    // In use by AutoMapper
    public LocationMappings()
    {
        CreateMap<Location, LocationDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.ToString())
            );

        
        CreateMap<Location, LocationLiteDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.ToString())
            )
            .ForMember(
                dest => dest.GpxPoint,
                opt 
                    => opt.MapFrom(src => new GpxPoint(src.Latitude, src.Longitude, 0))
            )
            ;

        // List to PaginatedResult mapping
        CreateMap<List<Location>, PaginatedResult<LocationDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => false));
    }
}