export interface Trail {
    id: string;
    name: string;
    normalizedName: string;
    description: string;
    distanceKm: number;
    elevationGainMeters: number;
    startLatitude: number;
    startLongitude: number;
    gpxFileUrl?: string;
    createdAt: string;
    updatedAt?: string;
}

export interface TrailPoint {
    latitude: number;
    longitude: number;
    elevationMeters: number;
    sequenceNumber: number;
}