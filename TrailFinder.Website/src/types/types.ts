// types/types.ts
import type { JSX } from "react";

export interface FilterOptions {
 difficulty: string[] | undefined;
 minDistance: number | undefined;
 maxDistance: number | undefined;
 region: string[] | undefined;
 terrainType: string[] | undefined;
}


export interface Route {
 imageUrl: JSX.Element;
 id: string;
 name: string;
 description: string;
 distanceMeters: number;
 distanceKm: number;
 elevationMeters: {
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
