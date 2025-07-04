-- Create enum for location types
CREATE TYPE location_type AS ENUM ('unknown', 'start', 'aid_station', 'checkpoint', 'end');

create table public.race_locations
(
    race_id       uuid                   not null,
    location_id   uuid                   not null,
    location_type public.location_type not null default 'unknown'::location_type,
    display_order numeric null,
    comment       text null,
    created_at    timestamp with time zone not null default now(),

    constraint race_locations_race_id_fkey foreign KEY (race_id) references races (id),
    constraint race_locations_location_id_fkey foreign KEY (location_id) references locations (id)
) TABLESPACE pg_default;