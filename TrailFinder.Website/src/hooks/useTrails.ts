// src/hooks/useTrails.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi'; // Correctly imports trailsApi
import type {Trail} from '@trailfinder/db-types'; // Ensure Trail type is correct

export function useTrails() {
    // Explicitly define the type for the data returned by useQuery
    // It is an array of Trail objects, as trailsApi.getAll() returns `response.data.items`
    return useQuery<Trail[], Error>({
        queryKey: ['allTrailsList'], // <-- **CRITICAL: Change queryKey to something unique like 'allTrailsList'**
        // This prevents conflict with any old, potentially misconfigured 'trails' key.
        queryFn: async () => {
            // trailsApi.getAll() correctly returns Promise<Trail[]>
            const trails = await trailsApi.getAll();
            // Optional: Sort trails here if you want a default order for client-side filtering
            return trails.sort((a, b) => a.name.localeCompare(b.name));
        },
        retry: 1,
        throwOnError: true // This will propagate errors to React Query's error handling
    });
}