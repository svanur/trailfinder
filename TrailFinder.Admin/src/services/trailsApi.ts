import { apiClient } from './api';
import { API_CONFIG } from '../config/api';
import type { Trail } from '@trailfinder/db-types';
import axios from 'axios';

// Define the type for the data sent to the API for creation
// 'description' is explicitly string | null
// Optional fields for update are marked with '?'
export type CreateTrailDto = {
    name: string;
    slug: string;
    description: string | null;
    distanceMeters: number | null;
    elevationGainMeters: number | null;
    elevationLossMeters: number | null;
    difficultyLevel: 'unknown' | 'easy' | 'moderate' | 'hard' | 'extreme';
    routeType: 'unknown' | 'circular' | 'outAndBack' | 'pointToPoint';
    terrainType: 'unknown' | 'flat' | 'rolling' | 'hilly' | 'mountainous';
    surfaceType: 'unknown' | 'trail' | 'paved' | 'mixed';
};

export const trailsApi = {
    getAll: async (latitude?: number | null, longitude?: number | null): Promise<Trail[]> => {
        let url = `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.TRAILS}`;
        console.info('getAll url 1', url);
        const params = new URLSearchParams();

        if (latitude !== null && latitude !== undefined && longitude !== null && longitude !== undefined) {
            params.append('userLatitude', latitude.toString());
            params.append('userLongitude', longitude.toString());
        }

        if (params.toString()) {
            url += `${url}?${params.toString()}`;
        }

        const response = await axios.get<Trail[]>(url);
        console.info('getAll url', url);
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
    },

    getById: async (id: string): Promise<Trail> => {
        const response = await apiClient.get<Trail>(`${API_CONFIG.ENDPOINTS.TRAILS}/${id}`);
        return response.data;
    },

    create: async (trailData: CreateTrailDto): Promise<Trail> => {
        const response = await apiClient.post<Trail>(`${API_CONFIG.ENDPOINTS.TRAILS}`, trailData);
        return response.data;
    },

    update: async (id: string, trailData: Partial<CreateTrailDto>): Promise<Trail> => {
        const response = await apiClient.put<Trail>(`${API_CONFIG.ENDPOINTS.TRAILS}/${id}`, trailData);
        return response.data;
    },

    delete: async (id: string): Promise<void> => {
        // Perform a soft delete by updating the 'is_active' column to false
        await apiClient.put(`${API_CONFIG.ENDPOINTS.TRAILS}/${id}`, { is_active: false });
    },
    
    uploadGpxFile: async (trailId: string, file: File): Promise<any> => {
        const formData = new FormData();
        formData.append('file', file);

        const response = await apiClient.post(
            `${API_CONFIG.ENDPOINTS.TRAILS}/${trailId}/gpx-file/upload`,
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );
        return response.data;
    },

    getGpxMetadata: async (trailId: string): Promise<any> => {
        const response = await apiClient.get(`${API_CONFIG.ENDPOINTS.TRAILS}/${trailId}/gpx-file/metadata`);
        return response.data;
    }
};