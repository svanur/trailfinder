import React from 'react';
import {
    IconMountain,
    IconRoute,
    IconRuler,
    IconArrowMoveUp,
    IconBarbell,
    IconCropLandscape,
} from '@tabler/icons-react';

import {
    Trail,
    DifficultyLevel, 
    RouteType, 
    TerrainType
} from '@trailfinder/db-types/database';

interface StatItemProps {
    icon: React.ReactNode;
    label: string;
    value: string;
}

interface TrailStatsProps {
    trail: Trail;
}

const StatItem: React.FC<StatItemProps> = ({ icon, label, value }) => (
    <div className="bg-gray-50 rounded-lg p-4 border border-gray-100">
        <div className="flex items-center gap-2 text-sm text-gray-600 mb-1">
            {icon}
            <span>{label}</span>
        </div>
        <div className="text-lg font-semibold">{value}</div>
    </div>
);


const TrailStats: React.FC<TrailStatsProps> = ({ trail }) => {
    const getDifficultyColor = (difficulty: DifficultyLevel) => {
        switch (difficulty) {
            case DifficultyLevel.Easy: return 'text-green-600';
            case DifficultyLevel.Moderate: return 'text-yellow-600';
            case DifficultyLevel.Hard: return 'text-orange-600';
            case DifficultyLevel.Expert: return 'text-red-600';
            default: return 'text-gray-600';
        }
    };

    const getRouteTypeIcon = (routeType: RouteType) => {
        switch (routeType) {
            case RouteType.Circular:
                return <IconRoute className="w-5 h-5" style={{ transform: 'rotate(90deg)' }} />;
            case RouteType.OutAndBack:
                return <IconRoute className="w-5 h-5" />;
            case RouteType.PointToPoint:
                return <IconRoute className="w-5 h-5" style={{ transform: 'rotate(45deg)' }} />;
            case RouteType.Unknown:
            default:
                return <IconRoute className="w-5 h-5" />;
        }
    };

    const getTerrainIcon = (terrainType: TerrainType) => {
        switch (terrainType) {
            case TerrainType.Flat:
                return <IconCropLandscape className="w-5 h-5" />;
            case TerrainType.Rolling:
                return <IconMountain className="w-5 h-5" stroke={1.5} />;
            case TerrainType.Hilly:
                return <IconMountain className="w-5 h-5" stroke={2} />;
            case TerrainType.Mountainous:
                return <IconMountain className="w-5 h-5" stroke={2.5} />;
            default:
                return <IconCropLandscape className="w-5 h-5" />;
        }
    };

    const formatDistance = (meters: number) => {
        return meters >= 1000
            ? `${(meters / 1000).toFixed(1)} km`
            : `${meters.toFixed(0)} m`;
    };

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            <StatItem
                icon={<IconRuler className="w-5 h-5" />}
                label="Vegalengd"
                value={formatDistance(trail.distance)}
            />

            <StatItem
                icon={<IconArrowMoveUp className="w-5 h-5" />}
                label="Hækkun"
                value={`${trail.elevationGainMeters.toFixed(0)} m`}
            />

            <StatItem
                icon={<IconBarbell className={`w-5 h-5 ${getDifficultyColor(trail.difficultyLevel)}`} />}
                label="Erfiðleikastig"
                value={trail.difficultyLevel}
            />

            <StatItem
                icon={getRouteTypeIcon(trail.routeType)}
                label="Leiðartegund"
                value={trail.routeType}
            />

            <StatItem
                icon={getTerrainIcon(trail.terrainType)}
                label="Landslag"
                value={trail.terrainType}
            />
        </div>
    );
};

export default TrailStats;
