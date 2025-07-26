// TrailFinder.DB\types\database.ts
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
    distanceMeters: number;
    distanceKm: number;
    elevationGainMeters: number;
    difficultyLevel: DifficultyLevel;
    routeType: RouteType;
    terrainType: TerrainType;
    surfaceType: SurfaceType;
    location: string;
    routeGeom?: any; // or more specific GeoJSON type if needed
    // missing start and end points
    distanceToUserKm: number | null;
    gpxFilePath: string | null;
    createdBy: string | null;
    createdAt: string; // ISO date string
    updatedBy: string | null;
    updatedAt: string; // ISO date string
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
    createdBy: string | null;
    updatedBy: string | null;
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
    OutAndBack: 'outAndBack',
    PointToPoint: 'pointToPoint'
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

export const SurfaceType = {
    Unknown: 'unknown',
    Trail: 'trail',
    Paved: 'paved',
    Mixed: 'mixed'
} as const;
export type SurfaceType = typeof SurfaceType[keyof typeof SurfaceType];

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
    createdBy: string | null;
}