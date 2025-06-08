using AutoMapper;
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
                opt => opt.MapFrom(src => src.GetStartCoordinates().Latitude)
            )
            .ForMember(
                dest => dest.StartPointLongitude,
                opt => opt.MapFrom(src => src.GetStartCoordinates().Longitude)
            );
    }
}
