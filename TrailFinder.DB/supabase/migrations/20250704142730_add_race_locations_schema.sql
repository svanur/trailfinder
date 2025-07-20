create table public.race_locations
(
    id            uuid                            not null default gen_random_uuid(), -- ID is the PK

    race_id       uuid                            not null,
    location_id   uuid                            not null,
    location_type public.location_type not null default 'unknown'::location_type,
    display_order numeric null,
    comment       text null,

    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by UUID REFERENCES auth.users(id) NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    updated_by UUID REFERENCES auth.users(id),

    constraint race_locations_pkey primary key (id),                                  -- PRIMARY KEY
    -- ADD a UNIQUE constraint on the 'race_id + location_id' combination
    constraint race_locations_unique_pair UNIQUE (race_id, location_id),
    constraint race_locations_race_id_fkey foreign KEY (race_id) references races (id),
    constraint race_locations_location_id_fkey foreign KEY (location_id) references locations (id)
);