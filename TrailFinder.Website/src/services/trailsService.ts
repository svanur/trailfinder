import { apiClient } from './apiClient'
import type {Trail} from "@trailfinder/db-types";

export const trailsService = {
  
    async getAllTrails(): Promise<Trail[]> {
        try {
            return await apiClient.get<Trail[]>('/trails');
        } catch (error){
            console.error('Failed to fetch trails', error);
            throw error;
        }
    },
    
    async getTrailBySlug(slug: string): Promise<Trail> {
        try {
            return await apiClient.get<Trail>(`/trails/${slug}`);
        } catch (error){
            console.error('Failed to fetch trail by slug: %s', slug, error);
            throw error;
        }
    },
    
    
};