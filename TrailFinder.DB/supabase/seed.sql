
-- Insert test user first (using Supabase's auth.users table)
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
    route_geom,
    start_point,
    web_url,
    gpx_file_path,
    user_id
) VALUES
      (
          'Mt Esja Ultra maraþon',
          'esja-ultra-marathon',
          'Maraþon keppnisleiðin í hlíðum Esjunnar.',
          45000,
          3353,
          'extreme',
          ST_GeomFromText('LINESTRING(-121.7603 46.7858, -121.7813 46.7959, -121.7923 46.8060)', 4326),
          ST_GeomFromText('POINT(-121.7603 46.7858)', 4326),
          'https://www.strava.com/activities/5495817983',
          '/storage/gpx/esja-ultra.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Mt Esja Ultra hálfmaraþon',
          'esja-ultra-half-marathon',
          'Hálfmaraþon keppnisleiðin í hlíðum Esjunnar',
          21000,
          1433,
          'hard',
          ST_GeomFromText('LINESTRING(-122.4058 47.6545, -122.4098 47.6578, -122.4148 47.6589)', 4326),
          ST_GeomFromText('POINT(-122.4058 47.6545)', 4326),
          'hhttps://connect.garmin.com/modern/course/253641069',
          '/storage/gpx/esja-ultra-half-marathon.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hengill Ultra 52',
          'hengill-ultra-52',
          'Hengill Ultra keppnishlaupið í Hveragerði',
          52000,
          1960,
          'hard',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
       'https://www.strava.com/routes/2829454378257603194',   
       '/storage/gpx/hengill-ultra-52.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 22km',
          'hvitasunnuhlaup-hauka-22',
          '22km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          21000,
          451,
          'moderate',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          'https://www.strava.com/activities/1586469545',   
          '/storage/gpx/hvitasunnuhlaup-hauka-22.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 17km',
          'hvitasunnuhlaup-hauka-17',
          '17km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          17000,
          137,
          'moderate',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          'https://connect.garmin.com/modern/course/60180539',   
          '/storage/gpx/hvitasunnuhlaup-hauka-17.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Hvítasunnuhlaup Hauka 14km',
          'hvitasunnuhlaup-hauka-14',
          '14km keppnisleiðin í Hvítasunnuhlaupi Hauka',
          14000,
          122,
          'easy',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          'https://connect.garmin.com/modern/course/60180128',   
          '/storage/gpx/hvitasunnuhlaup-hauka-14.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Puffin Run',
          'puffin-run',
          'Puffin Run keppnishlaupið í Vestmannaeygjum',
          20,
          295,
          'hard',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          'https://connect.garmin.com/modern/course/157776054',
          '/storage/gpx/puffin-run.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Bakgarður Náttúruhlaupa við Elliðavatn',
          'bakgardur-ellidavatn',
          'Bakgarður Náttúruhlaupa við Elliðavatn',
          6700,
          39,
          'easy',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          'https://www.strava.com/segments/25811198',   
          '/storage/gpx/bakgardur-ellidavatn.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      )
;