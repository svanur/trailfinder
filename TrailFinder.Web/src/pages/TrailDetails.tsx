import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Trail } from '@trailfinder/db-types/database';
import Layout from '../components/layout/Layout';
import { trails } from '../data'; // We'll import the mock data
import NotFound from '../components/NotFound';

const TrailDetails: React.FC = () => {
    const { normalizedName } = useParams<{ normalizedName: string }>();
    const [trail, setTrail] = useState<Trail | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        // Simulate API call with mock data
        const findTrail = () => {
            setTimeout(() => {
                try {
                    const foundTrail = trails.find(t => t.slug === normalizedName);
                    if (foundTrail) {
                        setTrail(foundTrail);
                    } else {
                        setError('Trail not found');
                    }
                } catch (err) {
                    setError(err instanceof Error ? err.message : 'Failed to load trail');
                } finally {
                    setLoading(false);
                }
            }, 500); // Add a small delay to simulate the network request
        };

        findTrail();
    }, [normalizedName]);
    
    if (loading) {
        return (
            <Layout>
                <div className="container mx-auto p-4">
                    <div className="animate-pulse">
                        <div className="h-8 bg-gray-200 rounded w-1/4 mb-4"></div>
                        <div className="h-4 bg-gray-200 rounded w-full mb-2"></div>
                        <div className="h-4 bg-gray-200 rounded w-full mb-2"></div>
                    </div>
                </div>
            </Layout>
        );
    }
    // In TrailDetails.tsx, update the error section:
    if (error || !trail) {
        return <NotFound />;
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

                    {/* Placeholder for the map */}
                    <div className="bg-gray-100 rounded-lg h-96 mb-6">
                        Map will go here
                    </div>

                    {/* Placeholder for elevation chart */}
                    <div className="bg-gray-100 rounded-lg h-48 mb-6">
                        Elevation chart will go here
                    </div>

                    {trail.gpx_file_path && (
                        <a
                            href={trail.gpx_file_path}
                            download
                            className="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                        >
                            <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                            </svg>
                            Download GPX
                        </a>
                    )}
                </div>
            </div>
        </Layout>
    );
};

export default TrailDetails;