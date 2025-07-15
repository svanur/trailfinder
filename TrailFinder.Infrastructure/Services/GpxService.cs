using System.Xml.Linq;
using NetTopologySuite.Geometries;
using TrailFinder.Application.Services;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.Gpx.Responses;

namespace TrailFinder.Infrastructure.Services;

public class GpxService : IGpxService
{
    private readonly GeometryFactory _geometryFactory;
    private readonly AnalysisService _analysisService;

    public GpxService(GeometryFactory geometryFactory, AnalysisService analysisService)
    {
        _geometryFactory = geometryFactory;
        _analysisService = analysisService;
    }

    public async Task<GpxInfoDto> ExtractGpxInfo(Stream gpxStream)
    {
        try
        {
            ValidateStream(gpxStream);

            var (trackPoints, ns) = await LoadTrackPoints(gpxStream);
            var points = trackPoints.Select(p => GpxPoint.FromXElement(p, ns)).ToList();
            
            var coordinates = points
                .Select(
                    p => new CoordinateZ(p.Longitude, p.Latitude, p.Elevation)
                )
                .Cast<Coordinate>()
                .ToArray();
            
            var routeGeom = _geometryFactory.CreateLineString(coordinates);
            
            var analysisResult = _analysisService.Analyze(points);
            
            return new GpxInfoDto(
                analysisResult.Distance,
                analysisResult.ElevationGain,
                analysisResult.DifficultyLevel,
                analysisResult.RouteType, 
                analysisResult.TerrainType,
                analysisResult.StartGpxPoint,
                analysisResult.EndGpxPoint,
                routeGeom
            );
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error processing GPX file: {ex.Message}", ex);
        }
    }

    private static void ValidateStream(Stream gpxStream)
    {
        if (gpxStream == null || gpxStream.Length == 0)
            throw new ArgumentException("GPX stream is null or empty");
    }

    private static async Task<(List<XElement> TrackPoints, XNamespace Namespace)> LoadTrackPoints(Stream gpxStream)
    {
        var doc = await XDocument.LoadAsync(gpxStream, LoadOptions.None, CancellationToken.None);
        var ns = doc.Root?.GetDefaultNamespace() ?? XNamespace.None;

        var trackPoints = doc.Descendants(ns + "trkpt").ToList();
        if (trackPoints.Count == 0)
            throw new InvalidOperationException("No track points found in GPX file");

        return (trackPoints, ns);
    }
}