// TrailFinder.DB\supabase\seeds\1-upload-gpx-files.js
const fs = require('fs');
const path = require('path');
const {Pool} = require('pg');

// PostgreSQL connection for querying trail IDs
const pool = new Pool({
    host: 'localhost',
    port: 54322,
    database: 'postgres',
    user: 'postgres',
    password: 'postgres'
});

async function printTheList() {
    const gpxDir = path.join(__dirname, 'gpx-files');
    const files = fs.readdirSync(gpxDir);
    
    console.log(`Found ${files.length} GPX files in ${gpxDir}.`);
    console.log(' ');

    let missingTrailCount = 0;
    let missingSlugs = [];
    for (const file of files) {
        if (file.endsWith('.gpx')) {
            const slug = path.parse(file).name;

            // Get trail ID from the database using slug
            const result = await pool.query('SELECT id, name FROM trails WHERE slug = $1 order by name', [slug]);
            if (result.rows.length === 0) {
                missingTrailCount += 1;
                missingSlugs.push(slug);
            }
        }
    }


    function getDashes(count) {
        return '-'.repeat(count); 
    }
    
    function padLabel(label, width) {
        return label.padEnd(width, ' ');
    }

    if (missingTrailCount > 0)
    {
        missingSlugs.forEach(slug => {
            console.log(slug);
        })


        const headingWidth = 15; 
        const labelWidth = 3; 
        console.log(' ');
        console.log(`${padLabel(`GPX files:`, headingWidth)} ${padLabel(`${files.length}`, labelWidth)} ${getDashes(files.length)}`);
        console.log(`${padLabel(`Trail & GPX:`, headingWidth)} ${padLabel(`${files.length - missingTrailCount}`, labelWidth)} ${getDashes(files.length - missingTrailCount)}`);
        console.log(`${padLabel(`Missing trails:`, headingWidth)} ${padLabel(`${missingTrailCount}`, labelWidth)} ${getDashes(missingTrailCount)}`);
        console.log(' ');

    }
}

async function main() {
    console.log(' ');
    console.log('All Trails having gpx file bot no entry in trails table');
    try {
        await printTheList();
    } catch (error) {
        console.error('An unexpected error occurred in main:', error); // More specific error message
    } finally {
        await pool.end();
    }
}

main()
    .then(r => console.log('Done'));