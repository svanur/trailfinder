// src/hooks/useTrails.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';

// src/hooks/useTrails.ts
export function useTrails() {
    return useQuery({
        queryKey: ['trails'],
        queryFn: async () => {
            const result = await trailsApi.getAll();
            console.log('Query function result:', result);  // Log the result
            return result;
        },
        retry: 1,
        throwOnError: true
    });

}
