import React, { useState } from 'react';
import { Trail } from '@trailfinder/db-types/database';
import { useGpxStorage } from '../hooks/useGpxStorage';

interface TrailGpxDownloadProps {
    trail: Trail;
}

interface GpxPoint {
    latitude: number;
    longitude: number;
    elevation: number;
}

interface TrailGpxInfo {
    distanceMeters: number;
    elevationGainMeters: number;
    startPoint: GpxPoint;
    endPoint: GpxPoint;
    routeGeom: [GpxPoint];
}

const TrailGpxDownload: React.FC<TrailGpxDownloadProps> = ({ trail }) => {
    const [isDownloading, setIsDownloading] = useState(false);
    const { getGpxContent } = useGpxStorage();

    const handleDownload = async () => {
        setIsDownloading(true);
        try {
            const gpxInfo: TrailGpxInfo = await getGpxContent(trail.id);

            const gpxPoints = gpxInfo.routeGeom.map((point: GpxPoint) => ({
                lat: point.latitude,
                lon: point.longitude,
                ele: point.elevation
            }));

            const gpxContent = `<?xml version="1.0" encoding="UTF-8"?>
<gpx version="1.1" creator="TrailFinder">
    <metadata>
        <name>${trail.name}</name>
    </metadata>
    <trk>
        <name>${trail.name}</name>
        <trkseg>
            ${gpxPoints.map((gpxPoint) =>
                `<trkpt lat="${gpxPoint.lat}" lon="${gpxPoint.lon}"><ele>${gpxPoint.ele}</ele></trkpt>`
            ).join('\n            ')}
        </trkseg>
    </trk>
</gpx>`;


            // Create and trigger download
            const blob = new Blob([gpxContent], { type: 'application/gpx+xml' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `${trail.slug}.gpx`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

        } catch (error) {
            console.error('Download failed:', error);
        } finally {
            setIsDownloading(false);
        }
    };

    return (
        <button
            onClick={handleDownload}
            disabled={isDownloading}
            className={`inline-flex items-center px-3 py-2 text-sm font-medium 
                       rounded-lg transition-colors focus:outline-none focus:ring-2 
                       focus:ring-blue-500 focus:ring-offset-2 
                       text-white bg-blue-600 hover:bg-blue-700 disabled:bg-blue-400`}
        >
            {isDownloading ? (
                <span className="animate-spin">⟳</span>
            ) : (
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
                          d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                </svg>
            )}
            <span className="hidden sm:inline ml-2">
                {isDownloading ? 'Hleð niður...' : 'Hlaða niður GPX'}
            </span>
        </button>
    );
};

export default TrailGpxDownload;
