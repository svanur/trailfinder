import React from 'react';
import { Link } from 'react-router-dom';
import { Trail } from '../../types';

interface TrailCardProps {
    trail: Trail;
}

const TrailCard: React.FC<TrailCardProps> = ({ trail }) => {
    return (
        <Link to={`/run/${trail.normalizedName}`}>
            <div className="border rounded-lg overflow-hidden shadow-lg hover:shadow-xl transition-shadow">
                <div className="p-4">
                    <h3 className="text-xl font-semibold">{trail.name}</h3>
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
                            {trail.distanceKm.toFixed(1)} km
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
                            {trail.elevationGainMeters}m hækkun
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
                                                {trail.startLatitude.toFixed(6)}°, {trail.startLongitude.toFixed(6)}°
                        </span>

                    </div>
                    <p className="mt-2 text-gray-700">{trail.description}</p>
                </div>
            </div>
        </Link>
    );
};

export default TrailCard;