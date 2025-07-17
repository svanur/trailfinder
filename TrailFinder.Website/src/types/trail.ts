// src/types/trail.ts

export declare const DifficultyLevel: {
    readonly Unknown: "unknown";
    readonly Easy: "easy";
    readonly Moderate: "moderate";
    readonly Hard: "hard";
    readonly Extreme: "extreme";
};
export type DifficultyLevel = typeof DifficultyLevel[keyof typeof DifficultyLevel];

export declare const RouteType: {
    readonly Unknown: "unknown";
    readonly Circular: "circular";
    readonly OutAndBack: "out-and-back";
    readonly PointToPoint: "point-to-point";
};
export type RouteType = typeof RouteType[keyof typeof RouteType];

export declare const TerrainType: {
    readonly Unknown: "unknown";
    readonly Flat: "flat";
    readonly Rolling: "rolling";
    readonly Hilly: "hilly";
    readonly Mountainous: "mountainous";
};
export type TerrainType = typeof TerrainType[keyof typeof TerrainType];

export declare const SurfaceType: {
    readonly Unknown: "unknown";
    readonly Trail: "trail";
    readonly Asphalt: "asphalt";
    readonly Sand: "sand";
    readonly Snow: "snow";
    readonly Ice: "ice";
};
export type SurfaceType = typeof SurfaceType[keyof typeof SurfaceType];

export interface TrailLocation {
    id: string;
    name: string;
    description?: string;
    latitude: number;
    longitude: number;
}

export interface Trail {
    id: string;
    name: string;
    slug: string;
    description: string;
    distance: number;
    elevationGain: number;
    difficultyLevel: DifficultyLevel;
    routeType: RouteType;
    terrainType: TerrainType;
    surfaceType: SurfaceType;
    webUrl?: string;
    hasGpx: boolean;
    trailLocations?: TrailLocation[];
    createdAt: string;
    updatedAt: string;
    userId: string;
}