// hooks/useGpxStorage.ts
import { supabase } from '../services/supabase';

export const useGpxStorage = () => {
    const uploadGpx = async (trailId: string, file: File) => {
        const { data, error } = await supabase.storage
            .from('gpx-files')
            .upload(`${trailId}/${file.name}`, file, {
                cacheControl: '3600',
                upsert: true
            });

        if (error) {
            throw error;
        }

        return data;
    };

    const downloadGpx = async (trailId: string, fileName: string) => {
        const { data, error } = await supabase.storage
            .from('gpx-files')
            .download(`${trailId}/${fileName}`);

        if (error) {
            throw error;
        }

        // Create download URL
        const url = URL.createObjectURL(data);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        URL.revokeObjectURL(url);
        document.body.removeChild(a);
    };

    return { uploadGpx, downloadGpx };
};
