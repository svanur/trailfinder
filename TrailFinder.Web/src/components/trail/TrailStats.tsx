// src/components/trail/TrailStats.tsx
import React from 'react';
import type { Trail } from '@trailfinder/db-types/database';
import {DistanceUnit, formatDistance} from '../../utils/distanceUtils';

interface TrailStatsProps {
    trail: Trail;
}

const TrailStats: React.FC<TrailStatsProps> = ({ trail }) => (
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
            {formatDistance(trail.distance, DistanceUnit.Kilometers)}
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
            {formatDistance(trail.elevationGainMeters, DistanceUnit.Meters)} hækkun
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
            {trail.startPointLatitude && trail.startPointLatitude}°,
            {trail.startPointLongitude && trail.startPointLongitude}°
        </span>
        {trail.webUrl && (
            <a
                href={trail.webUrl}
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
);

export default TrailStats;
