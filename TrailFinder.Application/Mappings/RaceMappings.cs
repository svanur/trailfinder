using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class RaceMappings : Profile
{
    // In use by AutoMapper
    public RaceMappings()
    {
        CreateMap<Race, RaceDto>()
            /*
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            */
            .ForMember(
                dest => dest.RaceLocations,
                opt => opt.MapFrom(src => src.RaceLocations)
            )
            ;

        CreateMap<PaginatedResult<Race>, PaginatedResult<RaceDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)) // Crucial: AutoMapper will use the Race -> RaceDto mapping for each item
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
        
        // List to PaginatedResult mapping
        /*
        CreateMap<List<Race>, PaginatedResult<RaceDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => false));
        */
    }
}
