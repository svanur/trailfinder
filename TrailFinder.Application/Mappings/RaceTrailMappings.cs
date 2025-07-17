using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class RaceTrailMappings : Profile
{
    // In use by AutoMapper
    public RaceTrailMappings()
    {
        /*
        CreateMap<RaceTrail, RaceTrailDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            ;
*/
        // List to PaginatedResult mapping
        CreateMap<PaginatedResult<RaceTrail>, PaginatedResult<RaceTrailDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
    }
}