// src/hooks/useTrail.ts
import { useQuery } from '@tanstack/react-query';
import {trailsService} from "../services/trailsService.ts";

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
            //const trail = await trailsApi.getBySlug(options.slug, options?.userLatitude, options?.userLongitude);
            return await trailsService.getTrailBySlug(options.slug);
        },
        enabled: !!options.slug
    });
}
