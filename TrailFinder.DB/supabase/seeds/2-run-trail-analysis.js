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

async function updateTrail() {
    try {
        const result = await pool.query('SELECT id, name FROM trails WHERE route_geom IS NOT NULL');

        for (const trail of result.rows) {
            const trailId = trail.id;
            const trailName = trail.name;

            try {
                // Get GPX info for the trail
                const trailResponse = await axios.get(`${API_BASE_URL}/trails/${trailId}`);
                const trailDetails = trailResponse.data;

                // Sanitize numeric values before sending
                const sanitizedGpxInfo = {
                    distance: sanitizeNumber(trailDetails.distance),
                    elevationGain: sanitizeNumber(trailDetails.elevationGain),
                    difficultyLevel: trailDetails.difficultyLevel,
                    routeType: trailDetails.routeType,
                    terrainType: trailDetails.terrainType,
                    startGpxPoint: trailDetails.startGpxPoint,
                    endGpxPoint: trailDetails.endGpxPoint,
                    routeGeom: trailDetails.routeGeom
                };

                // Update the trail with the GPX info
                //await axios.put(`${API_BASE_URL}/trails/${trailId}/info`, sanitizedGpxInfo);
                await axios.put(`${API_BASE_URL}/trails/${trailId}`, sanitizedGpxInfo);

                console.log(`Successfully updated information for "${trailName}" (${trailId})`);
                console.log({
                    name: trailName,
                    distance: sanitizedGpxInfo.distance,
                    elevation: sanitizedGpxInfo.elevationGain,
                    difficultyLevel: sanitizedGpxInfo.difficultyLevel,
                    routeType: sanitizedGpxInfo.routeType,
                    terrainType: sanitizedGpxInfo.terrainType,
                    hasRouteGeom: !!sanitizedGpxInfo.routeGeom,
                    startGpxPoint: sanitizedGpxInfo.startPoint,
                    endGpxPoint: sanitizedGpxInfo.endPoint
                   /* startPoint: {
                        latitude: sanitizedGpxInfo.startPoint.latitude,
                        longitude: sanitizedGpxInfo.startPoint.longitude,
                        elevation: sanitizedGpxInfo.startPoint.elevation
                    },
                    endPoint: {
                        latitude: sanitizedGpxInfo.endPoint.latitude,
                        longitude: sanitizedGpxInfo.endPoint.longitude,
                        elevation: sanitizedGpxInfo.endPoint.elevation
                    }*/
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
        console.log('Start trail analysis');
        await updateTrail();
        console.log('Trail analysis finished');
    } catch (error) {
        console.error('Error in main:', error);
        process.exit(1);
    }
}

main();
