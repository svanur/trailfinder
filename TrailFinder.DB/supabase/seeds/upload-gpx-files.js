const fs = require('fs');
const path = require('path');
const { createClient } = require('@supabase/supabase-js');
const { Pool } = require('pg');

// Supabase client configuration
const supabaseUrl = 'http://localhost:54321';  // Local Supabase URL
const supabaseKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImV4cCI6MTk4MzgxMjk5Nn0.EGIM96RAZx35lJzdJsyH-qQwv8Hdp7fsn3W0YpN81IU';
const supabase = createClient(supabaseUrl, supabaseKey);

// PostgreSQL connection for querying trail IDs
const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function uploadGpxFiles() {
    const gpxDir = path.join(__dirname, 'gpx-files');
    const files = fs.readdirSync(gpxDir);

    for (const file of files) {
        if (file.endsWith('.gpx')) {
            const slug = path.parse(file).name;
            const filePath = path.join(gpxDir, file);
            const fileContent = fs.readFileSync(filePath);
            const fileStats = fs.statSync(filePath);

            // Get trail ID from database using slug
            const result = await pool.query('SELECT id FROM trails WHERE slug = $1', [slug]);
            if (result.rows.length === 0) {
                console.error(`No trail found for slug: ${slug}`);
                continue;
            }

            const trailId = result.rows[0].id;

            // Upload file to Supabase Storage
            const { data, error } = await supabase.storage
                .from('gpx-files')
                .upload(`${trailId}.gpx`, fileContent, {
                    contentType: 'application/gpx+xml',
                    upsert: true,
                    duplex: 'half',
                    metadata: {
                        'trail-slug': slug,
                        'mimetype': 'application/gpx+xml',
                        'size': fileStats.size
                    }
                });

            if (error) {
                console.error(`Error uploading ${file}:`, error);
                continue;
            }

            // Update the has_gpx flag in the trails table
            await pool.query('UPDATE trails SET has_gpx = true WHERE id = $1', [trailId]);

            console.log(`Successfully uploaded ${file} as ${trailId}.gpx`);
        }
    }
}

async function main() {
    try {
        await uploadGpxFiles();
    } catch (error) {
        console.error('Error:', error);
    } finally {
        await pool.end();
    }
}

main();