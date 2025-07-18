create table public.race_trails
(
    id            uuid not null default gen_random_uuid(), -- ID is the PK

    race_id       uuid not null,
    trail_id      uuid not null,
    race_status   race_status not null,
    comment       text null,
    display_order numeric null,

    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by UUID REFERENCES auth.users(id) NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    updated_by UUID REFERENCES auth.users(id),

    constraint race_trails_pkey primary key (id),          -- PRIMARY KEY
    -- ADD a UNIQUE constraint on the 'race_id + trail_id' combination
    constraint race_trails_unique_pair UNIQUE (race_id, trail_id),
    constraint race_trails_race_id_fkey foreign KEY (race_id) references races (id),
    constraint race_trails_trail_id_fkey foreign KEY (trail_id) references trails (id)
);