// TrailFinder.Website\src\services\trailsApi.ts
import { apiClient } from './api';
import { API_CONFIG } from '../config/api';
import type { Trail } from '@trailfinder/db-types'; 
import axios from "axios";

export const trailsApi = {

    getAll: async (latitude?: number | null, longitude?: number | null): Promise<Trail[]> => { // Return Trail
        let url = `${API_CONFIG.ENDPOINTS.TRAILS}`;
        const params = new URLSearchParams();

        if (latitude !== null && latitude !== undefined && longitude !== null && longitude !== undefined) {
            params.append('userLatitude', latitude.toString());
            params.append('userLongitude', longitude.toString());
        }

        if (params.toString()) {
            url = `${url}?${params.toString()}`;
        }

        const response = await axios.get<Trail[]>(url);
        return response.data;
    },

    getBySlug: async (slug: string, latitude?: number | null, longitude?: number | null): Promise<Trail> => {
        try {
            let url = `${API_CONFIG.ENDPOINTS.TRAILS}/${slug}`;
            
            const params = new URLSearchParams();
            if (latitude !== null && latitude !== undefined && longitude !== null && longitude !== undefined) {
                params.append('userLatitude', latitude.toString());
                params.append('userLongitude', longitude.toString());
            }

            if (params.toString()) {
                url = `${url}?${params.toString()}`;
            }

            const response = await apiClient.get<Trail>(url);
            if (!response.data) {
                throw new Error('Trail not found');
            }

            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response?.status === 404) {
                throw new Error(`Trail with slug "${slug}" not found`);
            }
            throw error;
        }
    }

};
