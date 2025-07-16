// src/hooks/useTrail.ts
import { useQuery } from '@tanstack/react-query';
import { trailsApi } from '../services/trailsApi';
import axios from "axios";
import {Trail} from "@trailfinder/db-types/database";


export function useTrail(slug: string) {
    return useQuery<Trail, Error>({
        queryKey: ['trail', slug],
        queryFn: () => trailsApi.getBySlug(slug),
        enabled: !!slug, // Only run the query if we have a slug
        retry: (failureCount, error) => {
            // Don't retry on 404 errors
            if (axios.isAxiosError(error) && error.response?.status === 404) {
                return false;
            }
            // Retry other errors up to 3 times
            return failureCount < 3;
        }
    });
}