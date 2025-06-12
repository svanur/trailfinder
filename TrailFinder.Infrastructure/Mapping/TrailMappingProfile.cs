using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Entities;

namespace TrailFinder.Infrastructure.Mapping;

public class TrailMappingProfile : Profile
{
    public TrailMappingProfile()
    {
        // Base mapping for individual Trail to TrailDto
        CreateMap<Trail, TrailDto>();

        // Concrete collection mapping
        CreateMap<List<Trail>, List<TrailDto>>()
            .ConvertUsing((src, _, context) =>
                src.Select(trail => context.Mapper.Map<TrailDto>(trail)).ToList()
            );

        // Array mapping if needed
        CreateMap<Trail[], TrailDto[]>()
            .ConvertUsing((src, _, context) =>
                src.Select(trail => context.Mapper.Map<TrailDto>(trail)).ToArray()
            );

        // Mapping from List<Trail> to PaginatedResult<TrailDto>
        CreateMap<List<Trail>, PaginatedResult<TrailDto>>()
            .ConstructUsing((src, ctx) => new PaginatedResult<TrailDto>(
                ctx.Mapper.Map<List<TrailDto>>(src),
                src.Count,
                1,
                src.Count
            ));
        
        // Mapping from PaginatedResult<Trail> to PaginatedResult<TrailDto>
        CreateMap<PaginatedResult<Trail>, PaginatedResult<TrailDto>>()
            .ConstructUsing((src, ctx) => new PaginatedResult<TrailDto>(
                ctx.Mapper.Map<List<TrailDto>>(src.Items),
                src.TotalCount,
                src.PageNumber,
                src.PageSize
            ));

    }
}
