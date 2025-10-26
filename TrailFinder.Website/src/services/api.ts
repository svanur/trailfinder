// src/services/api.ts
import axios from 'axios';
import { API_CONFIG } from '../config/api';

export const apiClient = axios.create({
    baseURL: API_CONFIG.BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Adds an Authorization header to all requests
console.info('[api.ts] about to set up token interceptor');

//apiClient.interceptors.request.use((config) => {
    // Guard for environments without window/localStorage
    const hasStorage = typeof window !== 'undefined' && typeof window.localStorage !== 'undefined';
    let token: string | null = null;

    console.info('hasStorage:', hasStorage);
    if (hasStorage) {
        token = window.localStorage.getItem('bearerToken');
    }

    let config = { headers: { 'Authorization': token } };
    if (token) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = `Bearer ${token}`;
        console.info('[api.ts] token added to request', token);
    } else {
        console.info('[api.ts] no token found in localStorage');
    }

    console.info('config:', config);
//    return config;
//}, (error) => {
//    return Promise.reject(error);
//});

// Add response interceptor for error handling
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        // Handle errors (401, 403, etc.)
        return Promise.reject(error);
    }
);
