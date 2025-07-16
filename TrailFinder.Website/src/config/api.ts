// src/config/api.ts
export const API_CONFIG = {
    BASE_URL: import.meta.env.VITE_API_BASE_URL || '/',
    ENDPOINTS: {
        TRAILS: '/api/trails'
    }
};

