import { apiClient } from './apiClient'
import type {Trail} from "@trailfinder/db-types";

export const trailsService = {
  
    async getAllTrails(): Promise<Trail[]> {
        try {
            return await apiClient.get<Trail[]>('/api/v1/trails');
        } catch (error){
            console.error('Failed to fetch trails', error);
            throw error;
        }
    },
    
    async getTrailById(id: string): Promise<Trail> {
        try {
            return await apiClient.get<Trail>(`/trails/${id}`);
        } catch (error){
            console.error(`Failed to fetch trail by id ${id}`, error);
            throw error;
        }
    },
    
    
};