// src/types/trail.ts or wherever your Trail interface is defined
export interface Trail {
    id: string;
    parentId: string; 
    name: string;
    slug: string;
    description: string;
    distanceMeters: number;  
    elevationGainMeters: number;  
    difficultyLevel: number | null;  
    routeGeom: unknown;
    startPointLatitude: number | null;  
    startPointLongitude: number | null;  
    webUrl: string | null; 
    hasGpx: boolean;  
    createdAt: string;
    updatedAt: string;
    userId: string; 
}
