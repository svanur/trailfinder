using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.Trails.Responses;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class TrailMappings : Profile
{ // In use by AutoMapper
    public TrailMappings()
    {
        CreateMap<Trail, TrailDto>()

            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.ToString())
            )
/*
            //TODO: FIX: Error CS0854 : An expression tree may not contain a call or invocation that uses optional arguments
            .ForMember(
                destDto => destDto.StartGpxPoint,
                opt => opt.MapFrom(trail => new GpxPoint(trail.StartPoint.X, trail.StartPoint.Y, trail.StartPoint.Z))
            )

            .ForMember(
                destDto => destDto.EndGpxPoint,
                opt => opt.MapFrom(trail => new GpxPoint(trail.EndPoint.X, trail.EndPoint.Y, trail.EndPoint.Z))
            )
  */          
            /*
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
            );

        // List to PaginatedResult mapping
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