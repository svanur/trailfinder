// src/hooks/useTrails.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';
import type { Trail } from '@trailfinder/db-types/database';

export function useTrails() {
    return useQuery<Trail[], Error>({
        queryKey: ['trails'],
        queryFn: () => trailsApi.getAll()
    });
}