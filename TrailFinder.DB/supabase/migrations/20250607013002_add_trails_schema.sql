-- Enable PostGIS for spatial data
CREATE
EXTENSION IF NOT EXISTS postgis;

-- Create the 'trails' table
CREATE TABLE trails (
                        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                        name VARCHAR(255) NOT NULL,
                        slug VARCHAR(255) NOT NULL UNIQUE,
                        description TEXT,

                        distance NUMERIC(10,2),
                        elevation_gain INTEGER,

                        difficulty_level difficulty_level,
                        route_type route_type,
                        terrain_type terrain_type,
                        surface_type surface_type,

                        route_geom geometry(LINESTRINGZ, 4326),

                        web_url TEXT,

                        created_at TIMESTAMPTZ DEFAULT NOW(),
                        created_by UUID REFERENCES auth.users(id) NOT NULL,
                        updated_at TIMESTAMPTZ DEFAULT NOW(),
                        updated_by UUID REFERENCES auth.users(id)
);

-- Create index for spatial queries
CREATE INDEX trails_route_geom_idx ON trails USING GIST (route_geom);

-- Create updated_at trigger
CREATE
OR REPLACE FUNCTION update_updated_at_column()
    RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at
= NOW();
RETURN NEW;
END;
$$
language 'plpgsql';

CREATE TRIGGER update_trails_updated_at
    BEFORE UPDATE
    ON trails
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Enable RLS
ALTER TABLE trails ENABLE ROW LEVEL SECURITY;

-- Create RLS policies
CREATE
POLICY "Trails are viewable by everyone"
    ON trails FOR
SELECT
    USING (true);

CREATE
POLICY "Users can insert their own trails"
    ON trails FOR INSERT
    WITH CHECK (auth.uid() = created_by);

CREATE
POLICY "Users can update own trails"
    ON trails FOR
UPDATE
    USING (auth.uid() = created_by);

CREATE
POLICY "Users can delete own trails"
    ON trails FOR DELETE
USING (auth.uid() = created_by);

-- Create slugify function
CREATE
OR REPLACE FUNCTION slugify(text) RETURNS text AS $$
SELECT lower(regexp_replace(regexp_replace($1, '[^a-zA-Z0-9\-_]+', '-', 'g'), '-+', '-', 'g'));
$$
LANGUAGE SQL IMMUTABLE STRICT;
