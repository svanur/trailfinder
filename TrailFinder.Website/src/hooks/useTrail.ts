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
            const trail = await trailsApi.getBySlug(options.slug, options?.userLatitude, options?.userLongitude);
            console.log('get Trail by slug:', options.slug, trail);
            return trail;
        },
        enabled: !!options.slug
    });
}
