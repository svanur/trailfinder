-- Create an enum for difficulty_level
CREATE TYPE difficulty_level AS ENUM ('unknown', 'easy', 'moderate', 'hard', 'extreme');

-- Create enum for route_type
CREATE TYPE route_type AS ENUM ('unknown', 'circular', 'outAndBack', 'pointToPoint');

-- Create an enum for surface_type
CREATE TYPE surface_type AS ENUM ('unknown', 'trail', 'paved', 'mixed');

-- Create enum for terrain_type
CREATE TYPE terrain_type AS ENUM ('unknown', 'flat', 'rolling', 'hilly', 'mountainous');

-- Create an enum for race_status
CREATE TYPE race_status AS ENUM ('unknown', 'active', 'changed', 'cancelled', 'deprecated');

-- Create enum for location_type
CREATE TYPE location_type AS ENUM ('unknown', 'start', 'aidStation', 'checkpoint', 'end');
