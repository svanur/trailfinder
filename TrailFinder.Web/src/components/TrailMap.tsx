// src/components/TrailMap.tsx
import React, { useEffect, useRef } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';

interface TrailMapProps {
    gpxData: string;
    onHoverPoint?: (point: { lat: number; lng: number; elevation: number }) => void;
    highlightedPoint: { lat: number; lng: number; elevation: number } | null;
}

const TrailMap: React.FC<TrailMapProps> = ({ gpxData, onHoverPoint, highlightedPoint }) => {
    const mapRef = useRef<L.Map | null>(null);
    const mapContainerRef = useRef<HTMLDivElement>(null);
    const highlightMarkerRef = useRef<L.Marker | null>(null);

    useEffect(() => {
        if (!mapContainerRef.current) {
            return;
        }

        // Initialize the map if it doesn't exist
        if (!mapRef.current) {
            mapRef.current = L.map(mapContainerRef.current).setView([0, 0], 13);

            // Add OpenStreetMap tiles
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap contributors'
            }).addTo(mapRef.current);
        }

        // Parse GPX and add to map
        const parser = new DOMParser();
        const gpxDoc = parser.parseFromString(gpxData, 'text/xml');
        const points = Array.from(gpxDoc.getElementsByTagName('trkpt')).map(point => ({
            lat: parseFloat(point.getAttribute('lat') || '0'),
            lng: parseFloat(point.getAttribute('lon') || '0'),
            elevation: parseFloat(point.getElementsByTagName('ele')[0]?.textContent || '0')
        }));

        const polyline = L.polyline(points.map(p => [p.lat, p.lng]), {
            color: 'red',
            weight: 3,
        }).addTo(mapRef.current);

        // Add the mousemove event to the polyline
        if (onHoverPoint) {
            polyline.on('mousemove', (e) => {
                const closest = findClosestPoint(points, e.latlng);
                onHoverPoint(closest);
            });
        }

        // Fit map to show the entire route
        mapRef.current.fitBounds(polyline.getBounds());

        return () => {
            if (highlightMarkerRef.current) {
                highlightMarkerRef.current.remove();
            }
        };
    }, [gpxData, onHoverPoint]);

    // Handle highlighted point updates
    useEffect(() => {
        if (!mapRef.current) return;

        // Remove the existing highlight marker if it exists
        if (highlightMarkerRef.current) {
            highlightMarkerRef.current.remove();
        }

        // Add the new highlight marker if we have a point
        if (highlightedPoint) {
            highlightMarkerRef.current = L.marker([highlightedPoint.lat, highlightedPoint.lng], {
                icon: L.divIcon({
                    className: 'highlighted-point',
                    html: '<div style="width: 12px; height: 12px; background: #ff6b6b; border-radius: 50%; border: 2px solid white;"></div>'
                })
            }).addTo(mapRef.current);
        }
    }, [highlightedPoint]);

    return <div ref={mapContainerRef} style={{ height: '400px', width: '100%' }} />;
};

// Helper function to find the closest point
const findClosestPoint = (
    points: Array<{ lat: number; lng: number; elevation: number }>,
    latLng: L.LatLng
): { lat: number; lng: number; elevation: number } => {
    let minDist = Infinity;
    let closest = points[0];

    points.forEach(point => {
        const dist = Math.pow(point.lat - latLng.lat, 2) + Math.pow(point.lng - latLng.lng, 2);
        if (dist < minDist) {
            minDist = dist;
            closest = point;
        }
    });

    return closest;
};

export default TrailMap;
