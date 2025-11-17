// src/hooks/useTrails.ts

import { useQuery } from '@tanstack/react-query';
//import { trailsApi } from '../services/trailsApi';
import type { Trail } from '@trailfinder/db-types';
import { trailsService } from '../services/trailsService';

interface UseTrailsOptions {
    userLatitude?: number | null;
    userLongitude?: number | null;
}

export function useTrails(options?: UseTrailsOptions) {
    // Use a query key that includes the user location for re-fetching when the location changes
    return useQuery<Trail[], Error>({
        queryKey: ['trailsList', options?.userLatitude, options?.userLongitude],
        queryFn: async () => {
//            const trailsss = await trailsApi.getAll(/*options?.userLatitude, options?.userLongitude*/);
            const trails = await trailsService.getAllTrails();
            // No need to sort here if the backend already sorts by distance
            // If no user location, you can still sort by name as a default
            //if (!options?.userLatitude || !options?.userLongitude) {
            //    return trails.sort((a, b) => a.name.localeCompare(b.name));
            //}
  
            return trails; // Backend will return them sorted by distance
        },
        retry: 1,
        throwOnError: true,
    });
}