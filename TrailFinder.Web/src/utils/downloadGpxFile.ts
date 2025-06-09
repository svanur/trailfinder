import {supabase} from "../services/supabase.ts";

const downloadGpxFile = async (trailId: string) => {
    try {
        
        const { data, error } = await supabase.storage
            .from('gpx-files')
            .download(`${trailId}.gpx`);

        if (error) {
            console.error('Error downloading file:', error);
            return;
        }

        // Create a download link for the blob
        const blob = new Blob([data], { type: 'application/gpx+xml' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${trailId}.gpx`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    } catch (error) {
        console.error('Error:', error);
    }
};

export default downloadGpxFile;