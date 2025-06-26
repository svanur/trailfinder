
-- Insert a test user first (using Supabase's auth.users table)
INSERT INTO auth.users (id, email)
VALUES ('d0d8c29c-7456-4d82-a087-5a1256b092c9', 'test@example.com')
    ON CONFLICT (id) DO NOTHING;

-- Insert sample trails
INSERT INTO trails (
    name,
    slug,
    description,
    distance_meters,
    elevation_gain_meters,
    difficulty_level,
    route_type,
    terrain_type,
    web_url,
    has_gpx,
    user_id
)
VALUES
      (
          'Mt Esja Ultra maraþon',
          'esja-ultra-marathon',
          'Maraþon keppnisleiðin í hlíðum Esjunnar.',
       0,
0,
       'unknown',
          'unknown',
          'unknown',
          'https://www.strava.com/activities/5495817983',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Mt Esja Ultra hálfmaraþon',
          'esja-ultra-half-marathon',
          'Hálfmaraþon keppnisleiðin í hlíðum Esjunnar',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'hhttps://connect.garmin.com/modern/course/253641069',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hengill Ultra 52',
          'hengill-ultra-52',
          'Hengill Ultra keppnishlaupið í Hveragerði',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://www.strava.com/routes/2829454378257603194',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 22km',
          'hvitasunnuhlaup-hauka-22',
          '22km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://www.strava.com/activities/1586469545',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 17km',
          'hvitasunnuhlaup-hauka-17',
          '17km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://connect.garmin.com/modern/course/60180539',   
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 14km',
          'hvitasunnuhlaup-hauka-14',
          '14km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://connect.garmin.com/modern/course/60180128',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Puffin Run',
          'puffin-run',
          'Puffin Run keppnishlaupið í Vestmannaeygjum',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://connect.garmin.com/modern/course/157776054',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Bakgarður Náttúruhlaupa við Elliðavatn',
          'bakgardur-ellidavatn',
          'Bakgarður Náttúruhlaupa við Elliðavatn',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://www.strava.com/segments/25811198',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Skaftafell Ultra',
          'skaftafell-ultra',
          'Náttúruhlaup í Skaftafelli',
          0,
          0,
          'unknown',
          'unknown',
          'unknown',
          'https://www.strava.com/routes/3207311786703399086',
          false,
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      )
;
