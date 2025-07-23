// src/services/trailsApi.ts
import { apiClient } from './api';
import { API_CONFIG } from '../config/api';
import type { Trail } from '@trailfinder/db-types';
import axios from "axios";


export const trailsApi = {
    getAll: async (): Promise<Trail[]> => {
        const response = await axios.get<Trail[]>(`${API_CONFIG.ENDPOINTS.TRAILS}`);
        return response.data; // This MUST be the array directly
    },

    getBySlug: async (slug: string): Promise<Trail> => {
        try {
            const response = await apiClient.get<Trail>(
                `${API_CONFIG.ENDPOINTS.TRAILS}/${slug}`
            );

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
