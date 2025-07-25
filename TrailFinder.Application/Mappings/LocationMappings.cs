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
        /*
        CreateMap<Location, LocationDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            );
*/

        CreateMap<Location, LocationDto>()
            /*
              .ForMember(
                  dest => dest.ParentLocationDto,
                  opt => opt.MapFrom(src => src.ParentLocation))
              .ForMember(
                  dest => dest.ChildrenLocationsDto,
                  opt => opt.MapFrom(src => src.ChildrenLocations))
                      */
            ;
        
        CreateMap<Location, LocationLiteDto>()
  
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
  
  /*
            .ForMember(
                dest => dest.GpxPoint,
                opt 
                    => opt.MapFrom(src => new GpxPoint(src.Latitude, src.Longitude, 0))
            )
  */
            ;

        CreateMap<PaginatedResult<Location>, PaginatedResult<LocationDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
        
        CreateMap<PaginatedResult<Location>, PaginatedResult<LocationLiteDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
    }
}