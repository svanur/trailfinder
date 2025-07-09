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

async function getItems(items) {
    try {
        const table_name = items;
        const result = await pool.query('SELECT id, name FROM ' + table_name);

        for (const item of result.rows) {
            const item_id = item.id;
            const item_name = item.name;

            try {
                const url = `${API_BASE_URL}/${table_name}`;
                const response = await axios.get(url);
                const data = response.data;

                console.log(`Successfully got ${table_name} "${item_name}" (${item_id})`);
               
            } catch (error) {
                if (error.data)
                    console.error("Error getting " + table_name, error);
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

async function main(items) {
    try {
        console.log('Begin: Getting items from', items);
        await getItems(items);
        console.log('Finished: Getting', items);
    } catch (error) {
        console.error('Error in main:', error);
        process.exit(1);
    }
}

main('races');
