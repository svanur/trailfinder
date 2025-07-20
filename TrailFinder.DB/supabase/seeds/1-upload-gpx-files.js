// TrailFinder.DB\supabase\seeds\1-upload-gpx-files.js
const fs = require('fs');
const path = require('path');
const fetch = require('node-fetch'); // Keep node-fetch for the HTTP request itself
const FormData = require('form-data'); // <--- NEW: Import FormData from 'form-data'
const {Pool} = require('pg');

// --- API Configuration ---
const apiHostAddress = 'http://localhost:5263'; // Check your actual API port
const serviceUserId = '00000000-0000-0000-0000-000000000001'; // Valid UUID for the user performing the upload
// If your API requires authentication, uncomment and configure these:
// const authToken = 'YOUR_API_AUTH_TOKEN'; // e.g., a JWT token
// const authHeader = authToken ? { 'Authorization': `Bearer ${authToken}` } : {};


// PostgreSQL connection for querying trail IDs
const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function uploadGpxFilesViaApi() {
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
            // Corrected API URL (removed double /api)
            const uploadApiUrl = `${apiHostAddress}/api/trails/${trailId}/gpx-files/upload`;

            console.log(`Attempting to upload ${file} for Trail ID: ${trailId} via API: ${uploadApiUrl}`);

            // Prepare multipart/form-data using the 'form-data' package
            const formData = new FormData();
            formData.append('file', fileStream, {
                filename: file, // Use the original file name
                contentType: 'application/gpx+xml' // Explicitly set content type for the part
            });

            try {
                const response = await fetch(uploadApiUrl, {
                    method: 'POST',
                    body: formData,
                    // When using 'form-data' package, do NOT set 'Content-Type' header manually.
                    // FormData.getHeaders() will return the correct 'Content-Type' with boundary.
                    headers: {
                        // ...authHeader, // Uncomment if using authentication
                        ...formData.getHeaders() // <--- IMPORTANT: Get the headers from formData
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