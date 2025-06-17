using System.Xml.Linq;
using TrailFinder.Application.Services;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Infrastructure.Services;

public class GpxService : IGpxService
{
    private const double EarthRadiusMeters = 6371e3;
    private const double DegreesToRadians = Math.PI / 180;
    private const string BucketName = "gpx-files";
    private readonly ISupabaseStorageService _storageService;
    private readonly ITrailRepository _trailRepository;

    private record struct GpxPoint
    {
        private const double E7_TO_DECIMAL = 1e-7;
        public double Latitude { get; }
        public double Longitude { get; }
        public double? Elevation { get; }

        private GpxPoint(double latitude, double longitude, double? elevation = null)
        {
            // Convert from E7 format to decimal degrees
            Latitude = latitude * E7_TO_DECIMAL;
            Longitude = longitude * E7_TO_DECIMAL;
            Elevation = elevation;
        }

        public static GpxPoint FromXElement(XElement point, XNamespace ns)
        {
            var lat = ParseDoubleAttribute(point, "lat");
            var lon = ParseDoubleAttribute(point, "lon");
            var ele = ParseElevation(point, ns);

            return new GpxPoint(lat, lon, ele);
        }

        private static double ParseDoubleAttribute(XElement element, string attributeName)
        {
            var value = element.Attribute(attributeName)?.Value;
            return value != null ? double.Parse(value) : 0;
        }

        private static double? ParseElevation(XElement point, XNamespace ns)
        {
            var eleElement = point.Element(ns + "ele");
            return eleElement != null ? double.Parse(eleElement.Value) : null;
        }
    }


    public GpxService(ITrailRepository trailRepository, ISupabaseStorageService storageService)
    {
        _trailRepository = trailRepository;
        _storageService = storageService;
    }

    public async Task<GpxInfoDto> ExtractGpxInfo(Stream gpxStream)
    {
        try
        {
            ValidateStream(gpxStream);

            var (trackPoints, ns) = await LoadTrackPoints(gpxStream);
            var points = trackPoints.Select(p => GpxPoint.FromXElement(p, ns)).ToList();

            var totalDistance = CalculateTotalDistance(points);
            var elevationGain = CalculateElevationGain(points);
            var startPoint = points.First();
            var lastPoint = points.Last();

            return new GpxInfoDto(
                totalDistance,
                elevationGain,
                new GeoPoint(startPoint.Latitude, startPoint.Longitude),
                new GeoPoint(lastPoint.Latitude, lastPoint.Longitude)
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

    private static double CalculateTotalDistance(List<GpxPoint> points)
    {
        double totalDistance = 0;
        for (var i = 0; i < points.Count - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];
            totalDistance += CalculateDistance(point1, point2);
        }
        return totalDistance;
    }

    private static double CalculateElevationGain(List<GpxPoint> points)
    {
        double elevationGain = 0;
        for (var i = 0; i < points.Count - 1; i++)
        {
            if (points[i].Elevation.HasValue && points[i + 1].Elevation.HasValue)
            {
                var diff = points[i + 1].Elevation.Value - points[i].Elevation.Value;
                if (diff > 0) elevationGain += diff;
            }
        }
        return elevationGain;
    }

    
    private static double CalculateDistance(GpxPoint point1, GpxPoint point2)
    {
        const double radiansPerDegree = Math.PI / 180;
    
        // Check for valid coordinate ranges
        if (!IsValidCoordinate(point1) || !IsValidCoordinate(point2))
        {
            return 0;
        }
    
        var lat1Rad = point1.Latitude * radiansPerDegree;
        var lon1Rad = point1.Longitude * radiansPerDegree;
        var lat2Rad = point2.Latitude * radiansPerDegree;
        var lon2Rad = point2.Longitude * radiansPerDegree;

        var latDiff = lat2Rad - lat1Rad;
        var lonDiff = lon2Rad - lon1Rad;

        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
            
        // Check if 'a' is valid for further calculation
        if (double.IsNaN(a) || a > 1)
        {
            return 0;
        }

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return double.IsNaN(distance) ? 0 : distance;
    }

    private static bool IsValidCoordinate(GpxPoint point)
    {
        return point.Latitude is >= -90 and <= 90 &&
               point.Longitude is >= -180 and <= 180 &&
               !double.IsNaN(point.Latitude) && !double.IsNaN(point.Longitude);
    }


    private static double ToRadians(double degrees) => degrees * DegreesToRadians;

    private static double CalculateHaversineFormula(double lat1Rad, double lat2Rad, 
        double deltaLat, double deltaLon)
    {
        return Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
               Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
               Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
    }

    public async Task<Stream> GetGpxFileFromStorage(Guid trailId)
    {
        var trail = await _trailRepository.GetByIdAsync(trailId);
        if (trail == null) throw new TrailNotFoundException($"Trail not found with ID {trailId}");

        var fileName = $"{trail.Slug}/{trailId}.gpx";

        try
        {
            // First, check if the bucket exists
            /*
            var buckets = await _storageService
                .Storage
                .ListBuckets();

            if (buckets != null && buckets.All(b => b.Name != BucketName))
            {
                await _storageService
                    .Storage
                    .CreateBucket(BucketName);
            }

            // List files in the bucket to check if our file exists
            var files = await _storageService
                .From(BucketName)
                .List();

            if (files != null && files.All(f => f.Name != fileName))
            {
                throw new FileNotFoundException($"GPX file {fileName} not found in storage");
            }
            */

            var response = await _storageService
                .From(BucketName)
                .Download(fileName, null);

            if (response == null || response.Length == 0)
                throw new InvalidOperationException($"Downloaded file {fileName} is empty");

            return new MemoryStream(response);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
    }
    
}