using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class TrailMappings : Profile
{// In use by AutoMapper
    public TrailMappings()
    {
        CreateMap<Trail, TrailDto>()
            
            .ForMember(
                dest => dest.DistanceKm,
                opt => opt.MapFrom(src => Math.Round(src.DistanceMeters / 1000.0, 2) // Convert meters to KM for display, round to 2 decimals
                )
            )
            
            .ForMember(
                dest => dest.TrailLocations, 
                opt => opt.MapFrom(src => src.TrailLocations))
            ;

        // List to PaginatedResult mapping
        CreateMap<PaginatedResult<Trail>, PaginatedResult<TrailDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasPreviousPage, opt => opt.MapFrom(src => src.HasPreviousPage))
            .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage));
        
    }
}