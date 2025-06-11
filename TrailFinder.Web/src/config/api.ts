// src/config/api.ts
export const API_CONFIG = {
    BASE_URL: process.env.VITE_API_BASE_URL || '/api',
    ENDPOINTS: {
        TRAILS: '/trails'
    }
};
