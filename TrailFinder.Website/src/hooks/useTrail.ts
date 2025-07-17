// src/hooks/useTrail.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';

export function useTrail(slug: string) {
    return useQuery({
        queryKey: ['trail', slug],
        queryFn: () => trailsApi.getBySlug(slug),
        enabled: !!slug
    });
}
