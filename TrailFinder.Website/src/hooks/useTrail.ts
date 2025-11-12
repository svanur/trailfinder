// src/hooks/useTrail.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';

interface UseTrailOptions {
    slug: string;
    userLatitude?: number | null;
    userLongitude?: number | null;
}

export function useTrail(options: UseTrailOptions) {
    return useQuery({
        //queryKey: ['trail', slug],
        //queryFn: () => trailsApi.getBySlug(slug),
        queryKey: ['trail', options.slug, options?.userLatitude, options?.userLongitude],
        queryFn: async () => {
            return await trailsApi.getBySlug(options.slug, options?.userLatitude, options?.userLongitude);
        },
        enabled: !!options.slug
    });
}
