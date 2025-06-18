const axios = require('axios');
const { Pool } = require('pg');

const API_BASE_URL = 'http://localhost:5263/api';

// PostgreSQL connection for querying trails with GPX files
const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function updateTrailsGpxInfo() {
    try {
        // Get all trails that have GPX files
        const result = await pool.query('SELECT id, name FROM trails WHERE has_gpx = true');

        for (const trail of result.rows) {
            const trailId = trail.id;
            const trailName = trail.name;

            try {
                // Get GPX info for the trail
                const gpxInfoResponse = await axios.get(`${API_BASE_URL}/trails/${trailId}/info`);
                const gpxInfo = gpxInfoResponse.data;

                // Update the trail with the GPX info
                await axios.put(`${API_BASE_URL}/trails/${trailId}/info`, {
                    distanceMeters: gpxInfo.distanceMeters,
                    elevationGainMeters: gpxInfo.elevationGainMeters,
                    startPoint: gpxInfo.startPoint,
                    endPoint: gpxInfo.endPoint
                });

                console.log(`Successfully updated GPX info for trail ${trailName}`);
            } catch (error) {
                if (error.response) {
                    console.error(`Error updating trail ${trailName}:`, error.response.data);
                } else {
                    console.error(`Error updating trail ${trailName}:`, error.message);
                }
            }
        }
    } catch (error) {
        console.error('Database error:', error);
    } finally {
        await pool.end();
    }
}

async function main() {
    try {
        console.log('Begin updating trail information');
        await updateTrailsGpxInfo();
        console.log('Finished updating trail information');
    } catch (error) {
        console.error('Error in main:', error);
        process.exit(1);
    }
}

main();