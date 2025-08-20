import React from "react";
import {MapContainer, Marker, Polyline, Popup, TileLayer,} from "react-leaflet";
import {Loader, Paper, Title} from "@mantine/core";
import {Line} from "react-chartjs-2";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import {useTrail} from "../hooks/useTrail";

import {
    CategoryScale,
    Chart as ChartJS,
    Legend,
    LinearScale,
    LineElement,
    PointElement,
    Title as ChartTitle,
    Tooltip,
} from "chart.js";
import annotationPlugin from "chartjs-plugin-annotation";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    ChartTitle,
    Tooltip,
    Legend,
    annotationPlugin
);

export type RoutePointTuple = [number, number, number]; // [lon, lat, ele]

interface Trail {
    name: string;
    routeGeom: RoutePointTuple[];
}

interface TrailMapProps {
    slug: string;
    userLatitude?: number | null;
    userLongitude?: number | null;
}

// Byrjun + Endir tákn
const startIcon = L.icon({
    iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
    shadowSize: [41, 41],
});

const endIcon = L.icon({
    iconUrl:
        "https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
    shadowSize: [41, 41],
});

// Haversine fjarlægð
function haversineDistance(
    [lat1, lon1]: [number, number],
    [lat2, lon2]: [number, number]
) {
    const R = 6371; // km
    const dLat = ((lat2 - lat1) * Math.PI) / 180;
    const dLon = ((lon2 - lon1) * Math.PI) / 180;
    const a =
        Math.sin(dLat / 2) ** 2 +
        Math.cos((lat1 * Math.PI) / 180) *
        Math.cos((lat2 * Math.PI) / 180) *
        Math.sin(dLon / 2) ** 2;
    return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
}

export const TrailMap2: React.FC<TrailMapProps> = ({
                                                       slug,
                                                       userLatitude = null,
                                                       userLongitude = null,
                                                   }) => {
    const { data: trail, isLoading } = useTrail({
        slug,
        userLatitude,
        userLongitude,
    }) as { data?: Trail; isLoading: boolean };

    const [hoverIndex, setHoverIndex] = React.useState<number | null>(null);

    if (isLoading) return <Loader />;
    if (!trail?.routeGeom || trail.routeGeom.length === 0) {
        return <div>Engin gögn fundust</div>;
    }

    // [lat, lon] fyrir Leaflet
    const coords: [number, number][] = trail.routeGeom
        .filter(
            (pt): pt is RoutePointTuple =>
                Array.isArray(pt) &&
                typeof pt[0] === "number" &&
                typeof pt[1] === "number"
        )
        .map(([lon, lat]) => [lat, lon]);

    if (coords.length === 0) {
        return <div>Engin gild hnit fundust</div>;
    }

    // Fjarlægð + hæðarprófíl
    const distancePoints = coords.reduce<{ distance: number; ele: number }[]>(
        (acc, _, i) => {
            if (i === 0) {
                acc.push({ distance: 0, ele: trail.routeGeom[0][2] });
            } else {
                const dist = haversineDistance(coords[i - 1], coords[i]);
                acc.push({
                    distance: acc[i - 1].distance + dist,
                    ele: trail.routeGeom[i][2],
                });
            }
            return acc;
        },
        []
    );

    const chartData = {
        labels: distancePoints.map((p) => p.distance.toFixed(2)),
        datasets: [
            {
                label: "Hæð (m)",
                data: distancePoints.map((p) => p.ele),
                borderColor: "#1976d2",
                backgroundColor: "rgba(25, 118, 210, 0.1)",
                tension: 0.3,
            },
        ],
    };

    const chartOptions: any = {
        responsive: true,
        interaction: {
            mode: "index",
            intersect: false,
        },
        plugins: {
            tooltip: {
                enabled: true,
                callbacks: {
                    label: (context: any) => {
                        const km = distancePoints[context.dataIndex].distance.toFixed(2);
                        const ele = distancePoints[context.dataIndex].ele.toFixed(0);
                        return `${km} km — ${ele} m`;
                    },
                },
            },
            annotation: {
                annotations: {
                    hoverLine: {
                        type: "line",
                        borderColor: "#666",
                        borderWidth: 1,
                        scaleID: "x",
                        value: null,
                    },
                },
            },
        },
        // Virkar á mús og touch
        onHover: (event: any) => {
            const chart = event.chart;
            const activePoints = chart.getElementsAtEventForMode(
                event,
                "index",
                { intersect: false },
                true
            );
            if (activePoints.length) {
                const idx = activePoints[0].index;
                chart.options.plugins.annotation.annotations.hoverLine.value = chart.data.labels[idx];
                setHoverIndex(idx);
                chart.update("none");
            }
        },
    };

    const center: [number, number] = coords[0];

    return (
        <div style={{ display: "grid", gap: "1rem" }}>
            <Paper withBorder shadow="sm" p="sm">
                <Title order={4}>Kort af svæðinu</Title>
                <MapContainer center={center} zoom={13} style={{ height: 400 }}>
                    <TileLayer
                        attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a>'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    />
                    <Polyline
                        positions={coords}
                        pathOptions={{ color: "#1976d2", weight: 4 }}
                    />
                    <Marker position={coords[0]} icon={startIcon}>
                        <Popup>Byrjun</Popup>
                    </Marker>
                    <Marker position={coords[coords.length - 1]} icon={endIcon}>
                        <Popup>Endir</Popup>
                    </Marker>

                    {/* Live tracker punktur */}
                    {hoverIndex !== null && coords[hoverIndex] && (
                        <Marker
                            position={coords[hoverIndex]}
                            icon={L.divIcon({
                                className: "hover-marker",
                                html: `<div style="background:#ff5722;width:12px;height:12px;border-radius:50%;border:2px solid white"></div>`,
                                iconSize: [12, 12],
                                iconAnchor: [6, 6],
                            })}
                        />
                    )}
                </MapContainer>
            </Paper>

            <Paper withBorder shadow="sm" p="sm">
                <Title order={5}>Hæðarprófíl</Title>
                <Line data={chartData} options={chartOptions} />
            </Paper>
        </div>
    );
};
