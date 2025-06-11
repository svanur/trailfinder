export function parseGpxData(gpxData: string): GpxPoint[] | null {
    if (!gpxData) return null;

    const parser = new DOMParser();
    const gpxDoc = parser.parseFromString(gpxData, 'text/xml');
    return Array.from(gpxDoc.getElementsByTagName('trkpt')).map(point => ({
        lat: parseFloat(point.getAttribute('lat') || '0'),
        lng: parseFloat(point.getAttribute('lon') || '0'),
        elevation: parseFloat(point.getElementsByTagName('ele')[0]?.textContent || '0')
    }));
}
