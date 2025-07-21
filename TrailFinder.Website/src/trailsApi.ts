// src/services/trailsApi.ts
import type { Trail } from '@trailfinder/db-types';
import axios from "axios";
import type { PaginatedResponse } from './types/api';
import { apiClient } from './services/api';
import {API_CONFIG} from "./config/api.ts";

export const trailsApi = {
    getAll: async () => {
        try {
            const response = await apiClient.get<PaginatedResponse<Trail>>(API_CONFIG.ENDPOINTS.TRAILS);

            if (!response.data) {
                throw new Error('No data received from API');
            }

            return response.data.items;
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
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
