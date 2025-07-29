using System.Xml.Linq;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx.Responses;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Infrastructure.Services;

public class GpxService : IGpxService
{
    private readonly AnalysisService _analysisService;
    private readonly GeometryFactory _geometryFactory;
    private readonly IOsmLookupService _osmLookupService;

    public GpxService(
        GeometryFactory geometryFactory,
        AnalysisService analysisService,
        IOsmLookupService osmLookupService)
    {
        _geometryFactory = geometryFactory;
        _analysisService = analysisService;
        _osmLookupService = osmLookupService;
    }

    public async Task<GpxAnalysisResult> AnalyzeGpxTrack(Stream gpxStream)
    {
        try
        {
            ValidateStream(gpxStream);

            var (trackPoints, ns) = await LoadTrackPoints(gpxStream);
            var points = trackPoints.Select(p => GpxPoint.FromXElement(p, ns)).ToList();

            var coordinates = points
                .Select(p => new CoordinateZ(p.Longitude, p.Latitude, p.Elevation)
                )
                .Cast<Coordinate>()
                .ToArray();

            var routeGeom = _geometryFactory.CreateLineString(coordinates);

            // Determine SurfaceType using the new service
            var surfaceType = await _osmLookupService.DetermineSurfaceType(points); // Pass the points

            var analysisResult = _analysisService.Analyze(points, surfaceType); // Pass the determined surfaceType

            return new GpxAnalysisResult(
                analysisResult.TotalDistance,
                analysisResult.TotalElevationGain,
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