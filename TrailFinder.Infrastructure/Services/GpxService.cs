using TrailFinder.Application.Services;
using TrailFinder.Core.DTOs.Gpx;
using System.Xml.Linq;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Infrastructure.Services;

public class GpxService : IGpxService
{
    private readonly ISupabaseService _supabaseService;
    private const string BucketName = "gpx-files";

    public GpxService(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<Stream> GetGpxFileFromStorage(Guid trailId)
    {
        var fileName = $"{trailId}.gpx";
        
        try
        {
            // First, check if the bucket exists
            var buckets = await _supabaseService.Storage.ListBuckets();
            if (buckets != null && buckets.All(b => b.Name != BucketName))
            {
                await _supabaseService.Storage.CreateBucket(BucketName);
            }

            // List files in the bucket to check if our file exists
            var files = await _supabaseService.From(BucketName).List();
            if (files != null && files.All(f => f.Name != fileName))
            {
                throw new FileNotFoundException($"GPX file {fileName} not found in storage");
            }

            var response = await _supabaseService
                .From(BucketName)
                .Download(fileName, null);
            
            if (response == null || response.Length == 0)
            {
                throw new InvalidOperationException($"Downloaded file {fileName} is empty");
            }

            return new MemoryStream(response);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
    }

    public async Task<GpxInfoDto> ExtractGpxInfo(Stream gpxStream)
    {
        try
        {
            if (gpxStream == null || gpxStream.Length == 0)
            {
                throw new ArgumentException("GPX stream is null or empty");
            }

            var doc = await XDocument.LoadAsync(gpxStream, LoadOptions.None, CancellationToken.None);
            var ns = doc.Root?.GetDefaultNamespace() ?? XNamespace.None;

            var trackPoints = doc.Descendants(ns + "trkpt").ToList();
            if (trackPoints.Count == 0)
            {
                throw new InvalidOperationException("No track points found in GPX file");
            }

            // Calculate distance
            double totalDistance = 0;
            for (var i = 0; i < trackPoints.Count - 1; i++)
            {
                var point1 = trackPoints[i];
                var point2 = trackPoints[i + 1];
                
                var lat1 = double.Parse(point1.Attribute("lat")?.Value ?? "0");
                var lon1 = double.Parse(point1.Attribute("lon")?.Value ?? "0");
                var lat2 = double.Parse(point2.Attribute("lat")?.Value ?? "0");
                var lon2 = double.Parse(point2.Attribute("lon")?.Value ?? "0");
                
                totalDistance += CalculateDistance(lat1, lon1, lat2, lon2);
            }

            // Calculate elevation gain
            double elevationGain = 0;
            var elevations = trackPoints
                .Select(p => double.Parse(p.Element(ns + "ele")?.Value ?? "0"))
                .ToList();

            for (int i = 0; i < elevations.Count - 1; i++)
            {
                var diff = elevations[i + 1] - elevations[i];
                if (diff > 0) elevationGain += diff;
            }

            // Get start point
            var startPoint = trackPoints.First();
            var startLat = double.Parse(startPoint.Attribute("lat")?.Value ?? "0");
            var startLon = double.Parse(startPoint.Attribute("lon")?.Value ?? "0");

            return new GpxInfoDto(
                DistanceMeters: totalDistance,
                ElevationGainMeters: elevationGain,
                StartPoint: new GeoPoint(startLat, startLon)
            );
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error processing GPX file: {ex.Message}", ex);
        }
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371e3; // Earth's radius in meters
        var φ1 = lat1 * Math.PI / 180;
        var φ2 = lat2 * Math.PI / 180;
        var Δφ = (lat2 - lat1) * Math.PI / 180;
        var Δλ = (lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                Math.Cos(φ1) * Math.Cos(φ2) *
                Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }
}