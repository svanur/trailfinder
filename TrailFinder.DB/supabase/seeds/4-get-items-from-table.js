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

async function getItems(itemTable) {
    try {
        const query = `SELECT id FROM ${itemTable}`;
        const result = await pool.query(query);
        
        const url = `${API_BASE_URL}/${itemTable}`;
        console.log(`GET ${url}`);
        const response = await axios.get(url);
        const data = response.data;
        console.log(`Success: ${data.length} rows`);
        console.log(data);
        
        for (const item of result.rows) {
            const itemId = item.id;
            
            try {
                let itemUrl = `${url}/${itemId}`; 
                console.log(`GET ${itemUrl}`);
                const response = await axios.get(itemUrl);
                const itemData = response.data;
                console.log(`Success: ${itemData.name}`);
                console.log(' ');
            } catch (error) {
                console.error(`Error getting data for ${itemId}:`, error.code);
                if (error.response && error.response.status) {
                    console.error(error.response.status + " " + error.response.statusText);

                    if (error.response.data)
                        console.error(error.response.data.message);
                }
                return;
            }            
        }
        console.log(`Successfully got ${data.length} rows from table: ${itemTable}`);
        let jff = "";
        for (const item of result.rows) {
            jff += "-";
        }
        console.log(jff);
        
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

const tableName = 'trails';
main(tableName)
    .then(r => console.log('Done with table:', tableName));
