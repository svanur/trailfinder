const axios = require('axios');
const { Pool } = require('pg');

const API_BASE_URL = 'http://localhost:5263/api';

const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function updateTrailsGpxInfo() {
    try {
        const result = await pool.query('SELECT id, name FROM trails WHERE has_gpx = true');

        for (const trail of result.rows) {
            const trailId = trail.id;
            const trailName = trail.name;

            try {
                // Get GPX info for the trail
                const gpxInfoResponse = await axios.get(`${API_BASE_URL}/trails/${trailId}/info`);
                const gpxInfo = gpxInfoResponse.data;

                // Sanitize numeric values before sending
                const sanitizedGpxInfo = {
                    distanceMeters: sanitizeNumber(gpxInfo.distanceMeters),
                    elevationGainMeters: sanitizeNumber(gpxInfo.elevationGainMeters),
                    difficultyLevel: gpxInfo.difficultyLevel,
                    startPoint: gpxInfo.startPoint,
                    endPoint: gpxInfo.endPoint,
                    routeGeom: gpxInfo.routeGeom
                };

                // Update the trail with the GPX info
                await axios.put(`${API_BASE_URL}/trails/${trailId}/info`, sanitizedGpxInfo);

                console.log(`Successfully updated GPX info for trail "${trailName}" (${trailId})`);
                console.log({
                    name: trailName,
                    distance: sanitizedGpxInfo.distanceMeters,
                    elevation: sanitizedGpxInfo.elevationGainMeters,
                    difficultyLevel: sanitizedGpxInfo.difficultyLevel,
                    hasRouteGeom: !!sanitizedGpxInfo.routeGeom,
                    startPoint: {
                        latitude: sanitizedGpxInfo.startPoint.latitude,
                        longitude: sanitizedGpxInfo.startPoint.longitude,
                        elevation: sanitizedGpxInfo.startPoint.elevation
                    },
                    endPoint: {
                        latitude: sanitizedGpxInfo.endPoint.latitude,
                        longitude: sanitizedGpxInfo.endPoint.longitude,
                        elevation: sanitizedGpxInfo.endPoint.elevation
                    }
                });
            } catch (error) {
                if (error.response) {
                    console.error(`Error updating trail "${trailName}" (${trailId}):`, error.response.data);
                } else {
                    console.error(`Error updating trail "${trailName}" (${trailId}):`, error.message);
                }
            }
        }
    } catch (error) {
        console.error('Database error:', error);
    } finally {
        await pool.end();
    }
}

function sanitizeNumber(value) {
    if (!Number.isFinite(value)) {
        return 0; // or another appropriate default value
    }
    return value;
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
