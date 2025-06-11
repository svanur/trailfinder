// src/types/trail.ts
export interface Trail {
    id: string;
    parenId: string;
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
    has_gpx: boolean;
    created_at: string;
    updated_at: string;
    user_id: string;
}
