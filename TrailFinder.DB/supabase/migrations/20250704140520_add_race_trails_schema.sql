create table public.race_trails
(
    race_id       uuid                     not null,
    trail_id      uuid                     not null,
    status        race_status,
    comment       text null, -- E.g., "Route changed slightly this year due to mudslide"
    display_order numeric null,

    created_at    timestamp with time zone not null default now(),

    constraint race_trails_pkey primary key (race_id, trail_id),
    constraint race_trails_race_id_fkey foreign KEY (race_id) references races (id),
    constraint race_trails_trail_id_fkey foreign KEY (trail_id) references trails (id)
);