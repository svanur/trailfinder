import { useState, forwardRef, useImperativeHandle } from 'react';
import type { Trail } from '@trailfinder/db-types';
import { ActionIcon, Loader } from '@mantine/core';
import { IconDownload } from '@tabler/icons-react';
import { useGpxStorage } from '../hooks/useGpxStorage';

interface TrailGpxDownloadProps {
    trail: Trail;
}

interface TrailInfo {
    routeGeom: number[][]; // Corrected type to reflect the actual data
}

// Skilgreinum handle type fyrir ref
export interface TrailGpxDownloadHandle {
    handleDownload: () => Promise<void>;
}

const TrailGpxDownload = forwardRef<TrailGpxDownloadHandle, TrailGpxDownloadProps>(({ trail }, ref) => {
    const [isDownloading, setIsDownloading] = useState(false);
    const { getGpxContent } = useGpxStorage();

    const handleDownload = async () => {
        setIsDownloading(true);
        try {
            // Fetch the GPX data from the storage hook
            const gpxInfo: TrailInfo = await getGpxContent(trail.id);

            // Map the route geometry points to the GPX point format
            // Correcting the mapping to properly handle the nested array structure.
            const gpxPoints = gpxInfo.routeGeom.map((point: number[]) => ({
                lat: point[1], // Latitude is the second element
                lon: point[0], // Longitude is the first element
                ele: point[2]  // Elevation is the third element
            }));

            // Get the current time in ISO 8601 format
            const currentTime = new Date().toISOString();

            // Construct the GPX XML content string
            const gpxContent = `<?xml version="1.0" encoding="UTF-8"?>
<gpx version="1.1" creator="TrailFinder">
    <metadata>
        <name>${trail.name}</name>
        <author>
          <name>Hlaupaleiðir</name>
          <link href="https://www.hlaupaleidir.is"/>
        </author>
        <link href="https://www.hlaupaleidir.is/hlaup/${trail.slug}"/>
        <time>${currentTime}</time>
    </metadata>
    <trk>
        <name>${trail.name}</name>
        <type>running</type>
        <trkseg>
            ${gpxPoints.map((gpxPoint) =>
                `<trkpt lat="${gpxPoint.lat}" lon="${gpxPoint.lon}"><ele>${gpxPoint.ele}</ele></trkpt>`
            ).join('\n            ')}
        </trkseg>
    </trk>
</gpx>`;

            // Create a Blob from the GPX content
            const blob = new Blob([gpxContent], { type: 'application/gpx+xml' });

            // Create a temporary URL for the Blob and trigger a download
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `${trail.slug}.gpx`;
            document.body.appendChild(a);
            a.click();

            // Clean up the temporary URL and element
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

        } catch (error) {
            console.error('Download failed:', error);
            // Handle error state or show a notification to the user
        } finally {
            setIsDownloading(false);
        }
    };

    // Gerum handleDownload aðgengilegt í gegnum ref
    useImperativeHandle(ref, () => ({
        handleDownload
    }));

    return (
        <ActionIcon
            onClick={handleDownload}
            disabled={isDownloading}
            size="lg"
            variant="filled"
            color="blue"
            aria-label="Hlaða niður GPX skrá"
        >
            {isDownloading ? <Loader color="white" size="sm" /> : <IconDownload size={24} />}
        </ActionIcon>
    );
});

TrailGpxDownload.displayName = 'TrailGpxDownload';

export default TrailGpxDownload;