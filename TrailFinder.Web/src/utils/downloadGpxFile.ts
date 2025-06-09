import {supabase} from "../services/supabase.ts";

export const downloadGpxFile = async (trailId: string, triggerDownload = false) => {
    try {
        const { data, error } = await supabase.storage
            .from('gpx-files')
            .download(`${trailId}.gpx`);

        if (error) {
            console.error('Error downloading file:', error);
            return null;
        }

        const blob = new Blob([data], { type: 'application/gpx+xml' });

        if (triggerDownload) {
            // Create a download link for the blob
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `${trailId}.gpx`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
        }

        return blob;
    } catch (error) {
        console.error('Error:', error);
        return null;
    }
};

export default downloadGpxFile;