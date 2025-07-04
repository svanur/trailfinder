create table public.trail_locations
(
    trail_id       uuid                   not null,
    location_id   uuid                   not null,
    location_type public.location_type not null default 'unknown'::location_type,
    display_order numeric null,
    comment       text null,
    created_at    timestamp with time zone not null default now(),

    constraint trail_locations_trail_id_fkey foreign KEY (trail_id) references trails (id),
    constraint trail_locations_location_id_fkey foreign KEY (location_id) references locations (id)
) TABLESPACE pg_default;