-- Create enum for difficulty levels
CREATE TYPE difficulty_level AS ENUM ('unknown', 'easy', 'moderate', 'hard', 'extreme');

-- Create enum for race statuses
CREATE TYPE race_status AS ENUM ('unknown', 'deprecated', 'cancelled', 'changed', 'active');

-- Create enum for location types
CREATE TYPE location_type AS ENUM ('unknown', 'start', 'aid_station', 'checkpoint', 'end', 'start_and_end');

