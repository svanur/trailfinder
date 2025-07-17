// src/hooks/useTrails.ts
import {useQuery} from '@tanstack/react-query';
import {trailsApi} from '../services/trailsApi';

export function useTrails() {
    return useQuery({
        queryKey: ['trails'],
        queryFn: async () => {
            return await trailsApi.getAll();
        },
        retry: 1,
        throwOnError: true
    });

}
