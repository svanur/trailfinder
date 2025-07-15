export interface Database {
    public: {
        Tables: {
            trails: {
                Row: Trail;  // Return type when querying
                Insert: CreateTrailDTO;  // Type for inserting
                Update: Partial<CreateTrailDTO>;  // Type for updating
            };
            // Add other tables here
        };
    };
}

export interface Trail {
    id: string;
    parentId: string | null;
    name: string;
    slug: string;
    description: string;
    distance: number;
    elevationGainMeters: number;
    difficultyLevel: DifficultyLevel;
    routeType: RouteType;
    terrainType: TerrainType;
    startPointLatitude: number;
    startPointLongitude: number;
    endPointLatitude: number;
    endPointLongitude: number;
    routeGeom?: any; // or more specific GeoJSON type if needed - GpxPoint?
    webUrl?: string;
    hasGpx: boolean;
    createdAt: string; // ISO date string
    updatedAt: string; // ISO date string
    userId: string | null;
}

export enum DifficultyLevel {
    Easy = 'easy',
    Moderate = 'moderate',
    Hard = 'hard',
    Expert = 'expert'
}
export enum RouteType {
    Circular = 'circular',
    OutAndBack = 'out-and-back',
    PointToPoint = 'point-to-point',
    Unknown = 'unknown'
}

export enum TerrainType
{
    Flat = 'flat',
    Hilly = 'hilly',
    Rolling = 'rolling',
    Mountainous = 'mountainous'
}

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
