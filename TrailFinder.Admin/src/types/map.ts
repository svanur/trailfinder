// src/types/map.ts
// src/types/map.ts
import type {FeatureCollection, Geometry} from "geojson";
import type {PathOptions} from "leaflet";

export interface TrailGeoJSON extends FeatureCollection {
    features: Array<{
        type: 'Feature';
        geometry: Geometry;
        properties: {
            name: string;
            difficulty: 'easy' | 'moderate' | 'hard';
        };
    }>;
}

export interface TrailStyle extends PathOptions {
    color: string;
    weight: number;
    opacity: number;
}
