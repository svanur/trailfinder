export interface Database {
    public: {
        Tables: {
            trails: {
                Row: Trail;  // Return type when querying
                Insert: CreateTrailDTO;  // Type for inserting
                Update: Partial<CreateTrailDTO>;  // Type for updating //TODO: change to UpdateTrailDTO
            },
            locations: {
                Row: Location;
                Insert: Location; //TODO: change to CreateLocationDTO
                Update: Partial<Location>; //TODO: change to UpdateLocationDTO
            }
            ;
        };
    };
}

export interface Trail {
    id: string;
    name: string;
    slug: string;
    description: string;
    routeType: string;
    terrainType: string;
    surfaceType: string;
    location: string;
    distance: number;
    elevationGain: number;
    difficultyLevel: DifficultyLevel;
    routeGeom?: any; // or more specific GeoJSON type if needed
    webUrl?: string;
    hasGpx: boolean;
    createdAt: string; // ISO date string
    updatedAt: string; // ISO date string
    userId: string | null;
}

export interface Location {
    id: string;
    parentId: string | null;
    name: string;
    slug: string;
    description: string;
    latitude: number;
    longitude: number;
    createdAt: string; // ISO date string
    updatedAt: string; // ISO date string
    userId: string | null;
}
export const DifficultyLevel = {
    Unknown: 'unknown',
    Easy: 'easy',
    Moderate: 'moderate',
    Hard: 'hard',
    Extreme: 'extreme'
} as const;
export type DifficultyLevel = typeof DifficultyLevel[keyof typeof DifficultyLevel];

export const RouteType = {
    Unknown: 'unknown',
    Circular: 'circular',
    OutAndBack: 'out-and-back',
    PointToPoint: 'point-to-point'
} as const;
export type RouteType = typeof RouteType[keyof typeof RouteType];

export const TerrainType = {
    Unknown: 'unknown',
    Flat: 'flat',
    Rolling: 'rolling',
    Hilly: 'hilly',
    Mountainous: 'mountainous'
} as const;
export type TerrainType = typeof TerrainType[keyof typeof TerrainType];

export interface CreateTrailDTO {
    parentId: string;
    name: string;
    slug: string;
    description: string;
    distance: number;
    elevation_gain_meters: number;
    difficulty_level?: string;
    route_geom: unknown;
    start_point: unknown;
    web_url?: string;
    gpx_file_path?: string;
    user_id: string;
}