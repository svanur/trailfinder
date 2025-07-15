// src/services/api.ts
import axios from 'axios';
import { API_CONFIG } from '../config/api';

export const apiClient = axios.create({
    baseURL: API_CONFIG.BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Add a request interceptor for authentication if needed
apiClient.interceptors.request.use((config) => {
    // Add auth token or other headers here
    return config;
});

// Add response interceptor for error handling
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        // Handle errors (401, 403, etc.)
        return Promise.reject(error);
    }
);
