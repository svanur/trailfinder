// src/types/filters.ts (Create this new file)

export interface RangeFilter {
    min: number;
    max: number;
}

export interface TrailFilters {
    searchTerm: string;
    distance: RangeFilter;
    elevation: RangeFilter;
    surfaceTypes: string[];
    difficultyLevels: string[];
    routeTypes: string[];
    terrainTypes: string[];
    regions: string[];
}

// Initial state for filters
export const initialTrailFilters: TrailFilters = {
    searchTerm: '',
    distance: { min: 0, max: 50 }, // Max based on your RangeSlider
    elevation: { min: 0, max: 2000 }, // Max based on your RangeSlider
    surfaceTypes: [],
    difficultyLevels: [],
    routeTypes: [],
    terrainTypes: [],
    regions: [],
};
