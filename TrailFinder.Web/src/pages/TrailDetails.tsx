// src/pages/TrailDetails.tsx
import React, {useEffect, useMemo, useState} from 'react';
import { useParams } from 'react-router-dom';
import Layout from '../components/layout/Layout';
import { useTrail } from '../hooks/useTrail';
import TrailGpxDownload from '../components/TrailGpxDownload.tsx';
import TrailMap from '../components/TrailMap';
import ElevationProfile from '../components/ElevationProfile';
import downloadGpxFile from "../utils/downloadGpxFile.ts";

const TrailDetails: React.FC = () => {
    const { slug } = useParams<{ slug: string }>();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');
    const [gpxData, setGpxData] = useState<string>('');
    const [hoveredPoint, setHoveredPoint] = useState<number | null>(null);

    // Move useMemo before any conditional returns
    const parsedGpxData = useMemo(() => {
        if (!gpxData) return null;

        const parser = new DOMParser();
        const gpxDoc = parser.parseFromString(gpxData, 'text/xml');
        const points = Array.from(gpxDoc.getElementsByTagName('trkpt')).map(point => ({
            lat: parseFloat(point.getAttribute('lat') || '0'),
            lng: parseFloat(point.getAttribute('lon') || '0'),
            elevation: parseFloat(point.getElementsByTagName('ele')[0]?.textContent || '0')
        }));

        return points;
    }, [gpxData]);

    // Move useEffect before any conditional returns
    useEffect(() => {
        const loadGpxData = async () => {
            if (trail?.has_gpx) {
                const gpxBlob = await downloadGpxFile(trail.id, false);
                if (gpxBlob !== null) {
                    const text = await gpxBlob.text();
                    setGpxData(text);
                }
            }
        };

        loadGpxData();
    }, [trail]);

    const handleMapHover = (point: { lat: number; lng: number; elevation: number }) => {
        if (!parsedGpxData) return;
        const index = parsedGpxData.findIndex(p =>
            p.lat === point.lat && p.lng === point.lng
        );
        setHoveredPoint(index);
    };

    const handleProfileHover = (index: number) => {
        setHoveredPoint(index);
    };

    if (isLoading) {
        return (
            <Layout>
                <div>Best að hita smá upp...</div>
            </Layout>
        );
    }

    if (error) {
        return (
            <Layout>
                <div>Það er nú eitthvað að þessari :/</div>
            </Layout>
        );
    }

    if (!trail) {
        return (
            <Layout>
                <div>Þessi leið er eitthvað týnd :(</div>
            </Layout>
        );
    }

    return (
        <Layout>
            <div className="container mx-auto p-4">
                <div className="bg-white rounded-lg shadow-lg p-6">
                    <h1 className="text-3xl font-bold mb-4">{trail.name}</h1>

                    <div className="flex gap-4 mt-2 text-gray-600">
                        <span className="flex items-center">
                            <svg
                                className="w-4 h-4 mr-1"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M17 8l4 4m0 0l-4 4m4-4H3"
                                />
                            </svg>
                            {trail.distance_meters}km
                        </span>
                        <span className="flex items-center">
                            <svg
                                className="w-4 h-4 mr-1"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M5 10l7-7m0 0l7 7m-7-7v18"
                                />
                            </svg>
                            {trail.elevation_gain_meters}m
                        </span>
                        <span className="flex items-center">
                            <svg
                                className="w-4 h-4 mr-1"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
                                />
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
                                />
                            </svg>
                                              
                            {trail.start_point_latitude && trail.start_point_latitude}°, 
                            {trail.start_point_longitude && trail.start_point_longitude}°
                            
                        </span>

                        {trail.web_url && (
                            <a
                                href={trail.web_url}
                                target="_blank"
                                rel="noopener noreferrer"
                                className="flex items-center text-blue-600 hover:text-blue-800"
                            >
                                <svg
                                    className="w-4 h-4 mr-1"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14"
                                    />
                                </svg>
                                Skoða
                            </a>
                        )}
                    </div>


                    <p className="text-gray-700 mb-6">{trail.description}</p>

                    {trail.has_gpx && gpxData && parsedGpxData && (
                        <>
                            <div className="bg-white rounded-lg shadow-lg p-6 mb-6">
                                <TrailMap
                                    gpxData={gpxData}
                                    onHoverPoint={handleMapHover}
                                    highlightedPoint={hoveredPoint !== null ? parsedGpxData[hoveredPoint] : null}
                                />
                            </div>

                            <div className="bg-white rounded-lg shadow-lg p-6 mb-6">
                                <ElevationProfile
                                    elevationData={parsedGpxData.map(p => p.elevation)}
                                    onHoverPoint={handleProfileHover}
                                    highlightedIndex={hoveredPoint}
                                />
                            </div>
                        </>
                    )}


                    {trail.has_gpx && (
                        <TrailGpxDownload trail={trail} />
                    )}
                </div>
            </div>
        </Layout>
    );
};

export default TrailDetails;
