// src/hooks/useTrail.ts
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../services/supabase';
import type { Trail } from '@trailfinder/db-types/database';

export function useTrail(slug: string) {
    return useQuery({
        queryKey: ['trail', slug],
        queryFn: async (): Promise<Trail | null> => {
            const { data, error } = await supabase
                .from('trails')
                .select('*')
                .eq('slug', slug)
                .single();

            if (error) {
                throw error;
            }

            return data;
        },
        enabled: !!slug // Only run the query if we have a slug
    });
}