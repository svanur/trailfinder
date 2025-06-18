using AutoMapper;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Infrastructure.Mapping;

public class SupabaseMappingProfile : Profile
{
    public SupabaseMappingProfile()
    {
        CreateMap<SupabaseTrail, TrailDto>();
    }
}