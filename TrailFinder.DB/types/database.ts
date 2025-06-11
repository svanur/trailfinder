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
    parentId: string;
    name: string;
    slug: string;
    description: string;
    distanceMeters: number;
    elevationGainMeters: number;
    difficultyLevel: string | null;
    routeGeom: unknown;  // PostGIS type
    startPoint: unknown;  // PostGIS type
    startPointLatitude?: number; 
    startPointLongitude?: number; 
    webUrl: string | null;
    hasGpx: boolean;
    createdAt: string;
    updatedAt: string;
    userId: string;
}

export interface CreateTrailDTO {
    parentId: string;
    name: string;
    slug: string;
    description: string;
    distance_meters: number;
    elevation_gain_meters: number;
    difficulty_level?: string;
    route_geom: unknown;
    start_point: unknown;
    web_url?: string;
    gpx_file_path?: string;
    user_id: string;
}