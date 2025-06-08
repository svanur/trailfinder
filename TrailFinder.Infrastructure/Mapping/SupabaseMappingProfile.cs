using AutoMapper;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Infrastructure.Models;

namespace TrailFinder.Infrastructure.Mapping;

public class SupabaseMappingProfile : Profile
{
    public SupabaseMappingProfile()
    {
        CreateMap<SupabaseTrail, TrailDto>();
    }
}