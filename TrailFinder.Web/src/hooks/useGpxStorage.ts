// hooks/useGpxStorage.ts
import { apiClient } from '../services/api';
import { API_CONFIG } from '../config/api';

export const useGpxStorage = () => {
    const uploadGpx = async (trailId: string, file: File) => {
        const formData = new FormData();
        formData.append('file', file);

        const response = await apiClient.post(
            `${API_CONFIG.ENDPOINTS.TRAILS}/${trailId}/gpx`,
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );

        return response.data;
    };

    const downloadGpx = async (trailId: string, fileName: string) => {
        const response = await apiClient.get(
            `${API_CONFIG.ENDPOINTS.TRAILS}/${trailId}/gpx/${fileName}`,
            {
                responseType: 'blob'
            }
        );

        // Create download URL
        const url = URL.createObjectURL(response.data);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        URL.revokeObjectURL(url);
        document.body.removeChild(a);
    };

    const getGpxContent = async (trailId: string) => {
        const response = await apiClient.get(
            `${API_CONFIG.ENDPOINTS.TRAILS}/${trailId}/gpx`,
            {
                responseType: 'blob'
            }
        );
        return response.data;
    };

    return { uploadGpx, downloadGpx, getGpxContent };
};