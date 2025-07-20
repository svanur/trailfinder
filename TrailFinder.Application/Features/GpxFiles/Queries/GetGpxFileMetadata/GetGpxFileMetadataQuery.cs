// TrailFinder.Application/Features/GpxFiles/Queries/GetGpxFileMetadata/GetGpxFileMetadataQuery.cs
using MediatR;
using TrailFinder.Core.DTOs.GpxFile.Responses;

namespace TrailFinder.Application.Features.GpxFiles.Queries.GetGpxFileMetadata;

public record GetGpxFileMetadataQuery(Guid TrailId) : IRequest<GpxFileMetadataDto?>;
