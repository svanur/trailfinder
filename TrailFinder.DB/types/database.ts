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

export enum DifficultyLevel {
    Easy = 'easy',
    Moderate = 'moderate',
    Hard = 'hard',
    Expert = 'expert'
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