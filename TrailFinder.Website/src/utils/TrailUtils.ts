// src/utils/TrailUtils.ts
import { IconRoute, IconRefresh, IconArrowGuideFilled, IconArrowsDiff } from '@tabler/icons-react';

// Adjust path to your central types package
import {
    RouteType,        // This is the object { Unknown: 'unknown', Circular: 'circular', ... }
    DifficultyLevel,  // Same for DifficultyLevel
    TerrainType,      // Same for TerrainType
    SurfaceType       // Same for SurfaceType
} from '@trailfinder/db-types/types/database';

// --- Icon Mapping ---
// Note: We're mapping from the actual string value of the enum.
export const getRouteTypeIcon = (routeType: RouteType): React.FC<any> => {
    // We use the actual string values from the RouteType const object
    switch (routeType) {
        case RouteType.Circular:
            return IconRefresh; // Or IconCircle, IconRecycle
        case RouteType.OutAndBack:
            return IconArrowsDiff;
        case RouteType.PointToPoint:
            return IconArrowGuideFilled;
        case RouteType.Unknown:
        default:
            return IconRoute; // Default/fallback
    }
};

// --- Translation Mappings ---

    export const getRouteTypeTranslation = (routeType: RouteType): string => {
        switch (routeType) {
            case RouteType.Circular:
                return 'Hringur';
            case RouteType.OutAndBack:
                return 'Fram og til baka';
            case RouteType.PointToPoint:
                return 'A til B';
            case RouteType.Unknown:
            default:
                return 'Óþekkt';
        }
    };

export const getDifficultyLevelTranslation = (difficultyLevel: DifficultyLevel): string => {
    switch (difficultyLevel) {
        case DifficultyLevel.Easy:
            return 'Auðveld';
        case DifficultyLevel.Moderate:
            return 'Í meðallagi';
        case DifficultyLevel.Hard:
            return 'Erfið';
        case DifficultyLevel.Extreme:
            return 'Mjög erfið';
        case DifficultyLevel.Unknown:
        default:
            return 'Óskráð';
    }
};

export const getTerrainTypeTranslation = (terrainType: TerrainType): string => {
    switch (terrainType) {
        case TerrainType.Flat:
            return 'Flatlendi';
        case TerrainType.Rolling:
            return 'Rúllandi';
        case TerrainType.Hilly:
            return 'Hæðótt';
        case TerrainType.Mountainous:
            return 'Fjalllendi';
        case TerrainType.Unknown:
        default:
            return 'Óskráð';
    }
};

// Assuming you have SurfaceType similar to others
export const getSurfaceTypeTranslation = (surfaceType: SurfaceType): string => {
    switch (surfaceType) {
        case SurfaceType.Trail:
            return 'Utanvega';
        case SurfaceType.Paved:
            return 'Malbik';
        case SurfaceType.Mixed:
            return 'Blandað';
        case SurfaceType.Unknown:
        default:
            return 'Óskráð';
    }
};