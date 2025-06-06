import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Trail } from '../types';
import Layout from '../components/layout/Layout';
import { trails } from '../data'; // We'll import the mock data

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
                    const foundTrail = trails.find(t => t.normalizedName === normalizedName);
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
            }, 500); // Add a small delay to simulate network request
        };

        findTrail();
    }, [normalizedName]);

    // Rest of the component remains the same...
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

    if (error || !trail) {
        return (
            <Layout>
                <div className="container mx-auto p-4">
                    <div className="bg-red-50 border-l-4 border-red-500 p-4">
                        <p className="text-red-700">{error || 'Trail not found'}</p>
                    </div>
                </div>
            </Layout>
        );
    }

    return (
        <Layout>
            <div className="container mx-auto p-4">
                <div className="bg-white rounded-lg shadow-lg p-6">
                    <h1 className="text-3xl font-bold mb-4">{trail.name}</h1>

                    <div className="flex gap-6 mb-4">
                        <div className="flex items-center">
                            <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                            </svg>
                            <span className="text-lg">{trail.distanceKm.toFixed(1)} km</span>
                        </div>
                        <div className="flex items-center">
                            <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 10l7-7m0 0l7 7m-7-7v18" />
                            </svg>
                            <span className="text-lg">{trail.elevationGainMeters}m elevation gain</span>
                        </div>
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

                    {trail.gpxFileUrl && (
                        <a
                            href={trail.gpxFileUrl}
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