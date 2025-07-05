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

async function getLocations() {
    try {
        const result = await pool.query('SELECT id, name FROM locations');

        for (const item of result.rows) {
            const item_id = item.id;
            const item_name = item.name;

            try {
                const response = await axios.get(`${API_BASE_URL}/locations`);
                const data = response.data;

                console.log(`Successfully got location "${item_name}" (${item_id})`);
               
            } catch (error) {
                if (error.data)
                    console.error("Error getting location", error);
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
        console.log('Begin getting locations');
        await getLocations();
        console.log('Finished getting locations');
    } catch (error) {
        console.error('Error in main:', error);
        process.exit(1);
    }
}

main();
