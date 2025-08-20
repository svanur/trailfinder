import { useEffect, useRef, useState } from 'react';
import L from 'leaflet';
import type { GpxPoint } from '../types/GpxPoint';
import { Box, LoadingOverlay } from '@mantine/core';

interface TrailMapProps {
    points: GpxPoint[];
    onHoverPoint?: (point: GpxPoint) => void;
    highlightedPoint: GpxPoint | null;
    initialCenter?: [number, number];
    initialZoom?: number;
    height?: number | string;
}

export function TrailMap(
    {
        points,
        onHoverPoint,
        highlightedPoint,
        initialCenter,
        initialZoom = 13,
        height = 400
    }
    : TrailMapProps) {
    const [isLoading, setIsLoading] = useState(true);
    const mapRef = useRef<L.Map | null>(null);
    const mapContainerRef = useRef<HTMLDivElement>(null);
    const highlightMarkerRef = useRef<L.Marker | null>(null);
    const startMarkerRef = useRef<L.Marker | null>(null);
    const polylineRef = useRef<L.Polyline | null>(null);

    // Define the SVG for the start point from Tabler Icons
    const startSvg = `<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-flag-pin" width="32" height="32" viewBox="0 0 24 24" stroke-width="2" stroke="#228BE6" fill="#228BE6" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
        <path d="M10.978 20.372a2 2 0 1 0 2.032 .232l-.5 -2.604a1.854 1.854 0 0 1 -1.48 -.818l-4.113 -6.32a1.854 1.854 0 0 1 .49 -2.536a1.854 1.854 0 0 1 2.518 -.428a1.85 1.85 1 0 1 .843 1.258a1.854 1.854 0 0 1 -.488 2.535a1.85 1.85 0 0 1 -1.332 .628"></path>
        <path d="M12 21l0 -16"></path>
        <path d="M14 4h7v10h-5l-2 -2l-1 2"></path>
    </svg>`;

    // Define the SVG for the running figure from Tabler Icons
    const runnerSvg = `<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-run" width="32" height="32" viewBox="0 0 24 24" stroke-width="2" stroke="#E6501C" fill="none" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
        <path d="M13 4a1 1 0 1 0 2 0a1 1 0 0 0 -2 0"></path>
        <path d="M4 17l.5 -4l4 -2l1 6"></path>
        <path d="M14 21l-1 -4l-1 -1l-1 -1l-1 -2l.5 -1"></path>
        <path d="M16 11l2 1l.5 -2.5"></path>
        <path d="M7 10l2 -1l.5 -1"></path>
        <path d="M7 3l.5 2"></path>
    </svg>`;

    // Initialize map and polyline
    useEffect(() => {
        if (!mapContainerRef.current || !points.length) {
            setIsLoading(false);
            return;
        }

        if (!mapRef.current) {
            // Use initialCenter or calculate a sensible center from points
            const center = initialCenter || (() => {
                const midPoint = Math.floor(points.length / 2);
                return [points[midPoint].lat, points[midPoint].lng] as [number, number];
            })();

            mapRef.current = L.map(mapContainerRef.current).setView(center, initialZoom);

            // Add OpenStreetMap tile layer
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap contributors',
                maxZoom: 19
            }).addTo(mapRef.current);
        }

        // Create a custom SVG icon for the start point
        const startIcon = L.divIcon({
            className: 'start-marker',
            html: startSvg,
            iconSize: [32, 32],
            iconAnchor: [16, 32] // Anchor at the bottom of the flag pole
        });

        // Remove and recreate polyline to avoid duplicates
        if (polylineRef.current) {
            polylineRef.current.remove();
        }

        polylineRef.current = L.polyline(points.map(p => [p.lat, p.lng]), {
            color: '#228BE6', // Mantine blue
            weight: 4,
            opacity: 0.8,
            smoothFactor: 1
        }).addTo(mapRef.current);

        // Add start marker
        if (points.length > 0) {
            if (startMarkerRef.current) {
                startMarkerRef.current.remove();
            }
            startMarkerRef.current = L.marker([points[0].lat, points[0].lng], {
                icon: startIcon
            }).addTo(mapRef.current);
        }

        // Add hover interaction
        if (onHoverPoint && polylineRef.current) {
            polylineRef.current.on('mousemove', (e) => {
                const closest = findClosestPoint(points, e.latlng);
                onHoverPoint(closest);
            });
        }

        // Fit map bounds to show the entire route
        if (polylineRef.current) {
            mapRef.current.fitBounds(polylineRef.current.getBounds());
        }

        setIsLoading(false);

        // Cleanup function to remove all layers on unmount
        return () => {
            if (highlightMarkerRef.current) {
                highlightMarkerRef.current.remove();
            }
            if (startMarkerRef.current) {
                startMarkerRef.current.remove();
            }
            if (polylineRef.current) {
                polylineRef.current.remove();
            }
        };
    }, [points, initialCenter, initialZoom, onHoverPoint]);

    // Handle highlighted point updates
    useEffect(() => {
        if (!mapRef.current) return;

        if (highlightMarkerRef.current) {
            highlightMarkerRef.current.remove();
        }

        if (highlightedPoint) {
            // Create a custom SVG icon for the highlighted point, a running figure
            const highlightIcon = L.divIcon({
                className: 'hover-marker',
                html: runnerSvg,
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            });

            highlightMarkerRef.current = L.marker(
                [highlightedPoint.lat, highlightedPoint.lng],
                {icon: highlightIcon}
            ).addTo(mapRef.current);
        }
    }, [highlightedPoint]);

    return (
        <Box pos="relative" h={height}>
            <LoadingOverlay
                visible={isLoading}
                zIndex={1000}
                overlayProps={{radius: "sm", blur: 2}}
            />
            <div
                ref={mapContainerRef}
                style={{ height: '100%', width: '100%' }}
            />
        </Box>
    );
}

// Helper function to find the closest point on the path
const findClosestPoint = (
    points: GpxPoint[],
    latlng: L.LatLng
): GpxPoint => {
    let closestPoint = points[0];
    let minDist = Infinity;

    for (const point of points) {
        const dist = Math.pow(point.lat - latlng.lat, 2) +
            Math.pow(point.lng - latlng.lng, 2);
        if (dist < minDist) {
            minDist = dist;
            closestPoint = point;
        }
    }
    return closestPoint;
};
