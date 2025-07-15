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

async function getItems(table_name) {
    try {
        const query = `SELECT id FROM ${table_name}`;
        const result = await pool.query(query);
        console.log(query);
        console.log("Got: " + result.rows.length, 'items from', table_name, 'table');

        for (const item of result.rows) {
            const item_id = item.id;
            //const item_name = item.name;

            const url = `${API_BASE_URL}/${table_name}`;
            try {
                console.log(`GET ${url}`);
                const response = await axios.get(url);
                const data = response.data;
                console.log(`Successfully got data from ${table_name}`);
            } catch (error) {
                console.error(`Error getting data from ${table_name}:`, error.code);
                if (error.response && error.response.status) {
                    console.error(error.response.status + " " + error.response.statusText);

                    if (error.response.data)
                        console.error(error.response.data.message);
                }
                return;
            }

            try {
               // console.log(`Try to get item ${item_id} from ${table_name}`);
                const item_url = `${API_BASE_URL}/${table_name}/${item_id}`;
                console.log(`GET ${item_url}`);
                const item_response = await axios.get(item_url);
                const item_data = item_response.data;
                console.log(`Successfully got item from ${table_name}: ${item_data.id}`);

            } catch (error) {
                console.error(`Error getting item from ${table_name}`);
                if (error.data)
                    console.error("Error: " + error.data);
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
    } catch (error) {
        console.error('Error in main:', error);
        process.exit(1);
    }
}

main('trails')
    .then(r => console.log('Done'));
