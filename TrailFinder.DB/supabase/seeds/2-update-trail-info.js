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
                    difficultyLevel: sanitizeEnumValue(gpxInfo.difficultyLevel),
                    routeType: sanitizeEnumValue(gpxInfo.routeType),
                    terrainType: sanitizeEnumValue(gpxInfo.terrainType),
                    endPoint: gpxInfo.endPoint,
                    routeGeom: gpxInfo.routeGeom
                };
                
                // Update the trail with the GPX info
                await axios.put(`${API_BASE_URL}/trails/${trailId}/info`, sanitizedGpxInfo);

                console.log(`Successfully updated GPX info for trail "${trailName}" (${trailId})`);
                console.log({
                    name: trailName,
                    distance: `${(sanitizedGpxInfo.distanceMeters / 1000).toFixed(2)} km`,
                    elevation: `${sanitizedGpxInfo.elevationGainMeters.toFixed(0)} m`,
                    hasRouteGeom: !!sanitizedGpxInfo.routeGeom,
                    difficultyLevel: gpxInfo.difficultyLevel,
                    routeType: gpxInfo.routeType,
                    terrainType: gpxInfo.terrainType,
                    startPoint: sanitizedGpxInfo.startPoint,
                    endPoint: sanitizedGpxInfo.endPoint,
                });
            } catch (error) {
                if (error.response) {
                    console.error(`Error updating trail "${trailName}" (${trailId}):` + JSON.stringify(error.response.data));
                } else {
                    console.error(`Error updating trail: "${trailName}" (${trailId}): ` + error);
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

function sanitizeEnumValue(value) {
    return value ? value.toLowerCase() : null;
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
