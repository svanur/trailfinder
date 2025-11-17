// TrailFinder.DB/supabase/seeds/1-upload-gpx-files.js

import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';
import fetch from 'node-fetch';
import FormData from 'form-data';
import pg from 'pg';

const { Pool } = pg;

// Get the directory name using import.meta.url
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// --- API Configuration ---
const apiHostAddress = 'http://localhost:5263/api/v1';
const serviceUserId = '00000000-0000-0000-0000-000000000001';

// PostgreSQL connection for querying trail IDs
const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function uploadGpxFilesViaApi() {
    // Use the new __dirname variable here
    const gpxDir = path.join(__dirname, 'gpx-files');
    const files = fs.readdirSync(gpxDir);
    console.log(`Found ${files.length} GPX files in ${gpxDir}.`);

    for (const file of files) {
        if (file.endsWith('.gpx')) {
            const slug = path.parse(file).name;
            const filePath = path.join(gpxDir, file);
            const fileStream = fs.createReadStream(filePath);

            // Get trail ID from the database using slug
            const result = await pool.query('SELECT id FROM trails WHERE slug = $1', [slug]);
            if (result.rows.length === 0) {
                console.error(`No trail found for slug: ${slug}`);
                continue;
            }

            const trailId = result.rows[0].id;
            const uploadApiUrl = `${apiHostAddress}/trails/${trailId}/gpx-file/upload`;

            console.log(`Attempting to upload ${file} for Trail ID: ${trailId} via API: ${uploadApiUrl}`);

            // Prepare multipart/form-data using the 'form-data' package
            const formData = new FormData();
            formData.append('file', fileStream, {
                filename: file,
                contentType: 'application/gpx+xml' 
            });

            try {
                const response = await fetch(uploadApiUrl, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        // ...authHeader, // Uncomment if using authentication
                        ...formData.getHeaders() // <--- Get the headers from formData
                    }
                });

                if (!response.ok) {
                    const errorBody = await response.text();
                    console.error(`Error uploading ${file} (HTTP ${response.status}):`, errorBody);
                    continue;
                }

                const responseData = await response.json();
                console.log(`Successfully processed and uploaded ${file} for Trail ID: ${trailId}. Response:`, responseData);

            } catch (error) {
                console.error(`Network or API error uploading ${file}:`, error);
            }
        }
    }
}

console.log('GPX file upload process finished.'); // Moved here to ensure it always logs at the end of loop

async function main() {
    console.log('Starting GPX file upload via API...');
    try {
        await uploadGpxFilesViaApi();
    } catch (error) {
        console.error('An unexpected error occurred in main:', error); // More specific error message
    } finally {
        await pool.end();
    }
}

main()
    .then(r => console.log('Done'));