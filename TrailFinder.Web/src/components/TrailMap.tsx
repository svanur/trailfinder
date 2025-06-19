// src/components/TrailMap.tsx
import React, { useEffect, useRef } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';

interface TrailMapProps {
    points: GpxPoint[]; // Changed from gpxData: string
    onHoverPoint?: (point: GpxPoint) => void;
    highlightedPoint: GpxPoint | null;
}

const TrailMap: React.FC<TrailMapProps> = ({ points, onHoverPoint, highlightedPoint }) => {
    const mapRef = useRef<L.Map | null>(null);
    const mapContainerRef = useRef<HTMLDivElement>(null);
    const highlightMarkerRef = useRef<L.Marker | null>(null);
    const startMarkerRef = useRef<L.Marker | null>(null);

    useEffect(() => {
        if (!mapContainerRef.current || !points.length) return;

        if (!mapRef.current) {
            mapRef.current = L.map(mapContainerRef.current).setView([0, 0], 13);

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap contributors'
            }).addTo(mapRef.current);
        }

        const startIcon = L.divIcon({
            className: 'start-marker',
            html: `<span class="material-symbols-outlined">run_circle</span>`,
            iconSize: [32, 32],
            iconAnchor: [16, 16]
        });

        const polyline = L.polyline(points.map(p => [p.lat, p.lng]), {
            color: 'red',
            weight: 3,
        }).addTo(mapRef.current);

        // Add a start marker
        if (points.length > 0) {
            if (startMarkerRef.current) {
                startMarkerRef.current.remove();
            }
            startMarkerRef.current = L.marker([points[0].lat, points[0].lng], {
                icon: startIcon
            }).addTo(mapRef.current);
        }

        // Add a mousemove event to the polyline
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
            if (startMarkerRef.current) {
                startMarkerRef.current.remove();
            }
        };
    }, [points, onHoverPoint]);


    // Handle highlighted point updates
    useEffect(() => {
        if (!mapRef.current) return;

        if (highlightMarkerRef.current) {
            highlightMarkerRef.current.remove();
        }

        if (highlightedPoint) {
            const map = mapRef.current;
            highlightMarkerRef.current = L.marker([highlightedPoint.lat, highlightedPoint.lng], {
                icon: L.divIcon({
                    className: 'hover-marker',
                    html: `
            <span class="material-symbols-outlined">
              directions_run
            </span>
          `,
                    iconSize: [24, 24],
                    iconAnchor: [12, 12]
                })
            });

            if (map) {
                highlightMarkerRef.current.addTo(map);
            }
        }
    }, [highlightedPoint]);

    return <div ref={mapContainerRef} style={{ height: '400px', width: '100%' }} />;
};

const findClosestPoint = (
    points: Array<{ lat: number; lng: number; elevation: number }>,
    latlng: L.LatLng
): { lat: number; lng: number; elevation: number } => {
    let minDist = Infinity;
    let closest = points[0];

    points.forEach(point => {
        const dist = Math.pow(point.lat - latlng.lat, 2) + Math.pow(point.lng - latlng.lng, 2);
        if (dist < minDist) {
            minDist = dist;
            closest = point;
        }
    });

    return closest;
};

export default TrailMap;
