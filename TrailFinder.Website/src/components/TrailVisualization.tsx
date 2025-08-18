import React from 'react';
import type { GpxPoint } from '../types/GpxPoint.ts';
import {TrailMap} from "./TrailMap.tsx";
import {ElevationProfile} from "./ElevationProfile.tsx";

interface TrailVisualizationProps {
    hoveredPoint: number | null;
    onMapHover: (point: GpxPoint) => void;
    onProfileHover: (index: number) => void;
    parsedGpxData: GpxPoint[];
}

const TrailVisualization: React.FC<TrailVisualizationProps> = (
    {
        parsedGpxData,
        hoveredPoint,
        onMapHover,
        onProfileHover
    }
) => (
    <div className="space-y-4">
        <div className="bg-white rounded-lg border border-gray-200 shadow-sm p-6">
            <h2 className="text-xl font-semibold text-gray-900 mb-4">Kort</h2>
            <TrailMap
                points={parsedGpxData}
                onHoverPoint={onMapHover}
                highlightedPoint={hoveredPoint !== null ? parsedGpxData[hoveredPoint] : null}
            />
        </div>

        <div className="bg-white rounded-lg border border-gray-200 shadow-sm p-6">
            <h2 className="text-xl font-semibold text-gray-900 mb-4">Hæðarlínurit</h2>
            <ElevationProfile
                elevationData={parsedGpxData.map(p => p.elevation)}
                onHoverPoint={onProfileHover}
                highlightedIndex={hoveredPoint}
            />
        </div>
    </div>
);

export default TrailVisualization;
