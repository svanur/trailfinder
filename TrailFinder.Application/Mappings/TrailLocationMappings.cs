using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class TrailLocationMappings : Profile
{
    // In use by AutoMapper
    public TrailLocationMappings()
    {
        CreateMap<TrailLocation, TrailLocationDto>()
            /*
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            */
            .ForMember(
                dest => dest.TrailId,
                opt => opt.MapFrom(src => src.TrailId)
            )
            .ForMember(
                dest => dest.LocationId,
                opt => opt.MapFrom(src => src.LocationId)
            )
            /*
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            */
            .ForMember(
                dest => dest.Location,
                opt => opt.MapFrom(src => src.Location)
            )
            ;

        // List to PaginatedResult mapping
        CreateMap<PaginatedResult<TrailLocation>, PaginatedResult<TrailLocationDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
    }
}