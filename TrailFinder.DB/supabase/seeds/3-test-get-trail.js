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
        const result = await pool.query('SELECT * FROM trails WHERE route_geom IS NOT NULL LIMIT 1;');

        if (result.rowCount === 0) {
            console.info('No trail was found :( ');
            return;
        }
        
        const trail = result.rows[0];
        
        const trailId = trail.id;
        const trailName = trail.name;

        try {
            // Get info for the trail
            console.log('Get trail info for ' + trailName );
            const result = await axios.get(`${API_BASE_URL}/trails/${trailId}`);
            
            console.log(`Successfully got trail information for "${trailName}"`);
            console.log(result.data);
            
        } catch (error) {
            if (error.response) {
                console.error(`Error getting trail "${trailName}" (${trailId}):`, error.response.data);
            } else {
                console.error(`Error getting trail "${trailName}" (${trailId}):`, error.message);
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
