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
    name: string;
    slug: string;
    description: string;
    distance_meters: number;
    elevation_gain_meters: number;
    difficulty_level: string | null;
    route_geom: unknown;  // PostGIS type
    start_point: unknown;  // PostGIS type
    start_point_latitude?: number; 
    start_point_longitude?: number; 
    web_url: string | null;
    gpx_file_path: string | null;
    created_at: string;
    updated_at: string;
    user_id: string;
}

export interface CreateTrailDTO {
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