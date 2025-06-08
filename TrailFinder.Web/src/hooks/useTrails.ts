import { useQuery } from '@tanstack/react-query';
import { supabase } from '../services/supabase';
import type { Trail } from '@trailfinder/db-types/database';

export function useTrails() {
    return useQuery({
        queryKey: ['trails'],
        queryFn: async (): Promise<Trail[]> => {
            const { data, error } = await supabase
                .from('trails')
                .select('*')
                .order('name');

            if (error) {
                throw error;
            }

            return data;
        }
    });
}