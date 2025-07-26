// src/hooks/useTrails.ts (Update this file)
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';
import type { Trail } from '@trailfinder/db-types'; // Note: You'll need to update this Trail type

interface UseTrailsOptions {
    userLatitude?: number | null;
    userLongitude?: number | null;
}

export function useTrails(options?: UseTrailsOptions) {
    // Use a query key that includes the user location for re-fetching when location changes
    return useQuery<Trail[], Error>({ // Use TrailDto here
        queryKey: ['trailsList', options?.userLatitude, options?.userLongitude],
        queryFn: async () => {
            const trails = await trailsApi.getAll(options?.userLatitude, options?.userLongitude);
            // No need to sort here if backend already sorts by distance
            // If no user location, you can still sort by name as a default
            if (!options?.userLatitude || !options?.userLongitude) {
                return trails.sort((a, b) => a.name.localeCompare(b.name));
            }
            return trails; // Backend will return them sorted by distance
        },
        retry: 1,
        throwOnError: true,
    });
}