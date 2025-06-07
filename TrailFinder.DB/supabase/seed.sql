
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
    gpx_file_path,
    user_id
) VALUES
      (
          'Mount Rainier Wonderland Trail',
          'mount-rainier-wonderland-trail',
          'A spectacular circumnavigation of Mount Rainier through subalpine meadows and valleys.',
          12000.50,
          1250,
          'hard',
          ST_GeomFromText('LINESTRING(-121.7603 46.7858, -121.7813 46.7959, -121.7923 46.8060)', 4326),
          ST_GeomFromText('POINT(-121.7603 46.7858)', 4326),
          '/gpx/wonderland-trail.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Discovery Park Loop',
          'discovery-park-loop',
          'A scenic urban trail with forest, meadows, and beach access in Seattle.',
          4200.75,
          120,
          'easy',
          ST_GeomFromText('LINESTRING(-122.4058 47.6545, -122.4098 47.6578, -122.4148 47.6589)', 4326),
          ST_GeomFromText('POINT(-122.4058 47.6545)', 4326),
          '/gpx/discovery-loop.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      ),
      (
          'Tiger Mountain Trail',
          'tiger-mountain-trail',
          'Popular hiking destination with multiple difficulty levels and great views of the Puget Sound.',
          8500.25,
          800,
          'moderate',
          ST_GeomFromText('LINESTRING(-121.9713 47.5093, -121.9813 47.5159, -121.9913 47.5259)', 4326),
          ST_GeomFromText('POINT(-121.9713 47.5093)', 4326),
          '/gpx/tiger-mountain.gpx',
          'd0d8c29c-7456-4d82-a087-5a1256b092c9'
      );