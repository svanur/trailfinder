using AutoMapper;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.GpxFile.Responses;
using TrailFinder.Core.DTOs.Race.Response;
using TrailFinder.Core.Entities;

namespace TrailFinder.Application.Mappings;

public class GpxFileMappings : Profile
{
    // In use by AutoMapper
    public GpxFileMappings()
    {
        CreateMap<GpxFile, GpxFileMetadataDto>() ;
    }
}
