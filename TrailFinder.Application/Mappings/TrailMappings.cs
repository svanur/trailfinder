using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class TrailMappings : Profile
{
    public TrailMappings()
    {
        CreateMap<Trail, TrailDto>()
            .ForMember(
                dest => dest.StartPointLatitude,
                opt => opt.MapFrom(src =>
                    src.GetStartCoordinates() != null ? src.GetStartCoordinates()!.Value.Latitude ?? 0 : 0)
            )
            .ForMember(
                dest => dest.StartPointLongitude,
                opt => opt.MapFrom(src =>
                    src.GetStartCoordinates() != null ? src.GetStartCoordinates()!.Value.Longitude ?? 0 : 0)
            )
            .ForMember(
                dest => dest.EndPointLatitude,
                opt => opt.MapFrom(src =>
                    src.GetEndCoordinates() != null ? src.GetEndCoordinates()!.Value.Latitude ?? 0 : 0)
            )
            .ForMember(
                dest => dest.EndPointLongitude,
                opt => opt.MapFrom(src =>
                    src.GetEndCoordinates() != null ? src.GetEndCoordinates()!.Value.Longitude ?? 0 : 0)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.ToString())
            )
            .ForMember(
                dest => dest.ParentId,
                opt => opt.MapFrom(src => src.ParentId.HasValue ? src.ParentId.Value.ToString() : string.Empty)
            )
            /*
            .ForMember(
                dest => dest.RouteType,
                opt => opt.MapFrom(src => src.RouteType)
            )
            .ForMember(
                dest => dest.TerrainType,
                opt => opt.MapFrom(src => src.TerrainType)
            )
            */
            .ForMember(
                dest => dest.RouteGeom,
                opt => opt.MapFrom(src => src.RouteGeom)
            )
            .ForMember(
                dest => dest.WebUrl,
                opt => opt.MapFrom(src => src.WebUrl)
            )
            .ForMember(
                dest => dest.HasGpx,
                opt => opt.MapFrom(src => src.HasGpx)
            )
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId.ToString())
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt)
            )
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt)
            );

        // List to PaginatedResult mapping remains the same
        CreateMap<List<Trail>, PaginatedResult<TrailDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => false));
    }
}