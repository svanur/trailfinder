// src/services/trailsApi.ts
import { apiClient } from './api';
import { API_CONFIG } from '../config/api';
import type { Trail } from '@trailfinder/db-types/database';
import type { ApiResponse } from '../types/api';
import axios from "axios";

export const trailsApi = {
    getAll: async (): Promise<Trail[]> => {
        const response = await apiClient.get<ApiResponse<Trail[]>>(
            API_CONFIG.ENDPOINTS.TRAILS
        );
        return response.data.data;
    },

    getBySlug: async (slug: string): Promise<Trail> => {
        try {
            const response = await apiClient.get<ApiResponse<Trail>>(
                `${API_CONFIG.ENDPOINTS.TRAILS}/${slug}`
            );
            return response.data.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response?.status === 404) {
                // You can either return null or throw a custom error
                return null as unknown as Trail; // Type assertion to maintain compatibility
                // Or throw new Error('Trail not found');
            }
            throw error;
        }
    }

};
