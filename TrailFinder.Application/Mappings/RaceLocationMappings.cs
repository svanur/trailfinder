using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class RaceLocationMappings : Profile
{
    // In use by AutoMapper
    public RaceLocationMappings()
    {
        CreateMap<RaceLocation, RaceLocationDto>()
            
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.RaceId,
                opt => opt.MapFrom(src => src.RaceId)
            )
            .ForMember(
                dest => dest.LocationId,
                opt => opt.MapFrom(src => src.LocationId)
            )
            
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            
            .ForMember(
                dest => dest.Location,
                opt => opt.MapFrom(src => src.Location)
            )
            ;

        // List to PaginatedResult mapping
        CreateMap<List<RaceLocation>, PaginatedResult<RaceLocationDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => false));
    }
}