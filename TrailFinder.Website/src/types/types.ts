import type { JSX } from "react";

// types/types.ts
export interface Route {
    imageUrl: JSX.Element;
    id: string;
    name: string;
    description: string;
    distance: number;
    elevation: {
        gain: number;
        loss: number;
    };
    difficulty: 'easy' | 'moderate' | 'hard';
    terrainType: string[];
    region: string;
    gpxFile: string;
    coordinates: {
        start: [number, number];
        end: [number, number];
    };
    geometry: GeoJSON.LineString;
}

export interface FilterOptions {
    difficulty: string[] | undefined;
    minDistance: number | undefined;
    maxDistance: number | undefined;
    region: string[] | undefined;
    terrainType: string[] | undefined;
}