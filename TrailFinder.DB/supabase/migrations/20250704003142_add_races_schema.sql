create table public.races
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    location_id       UUID null,
    name              text null,
    slug              text null,
    description       text null,
    status            race_status,
    recurring_month   smallint null,
    recurring_week    smallint null,
    recurring_weekday smallint null,

    user_id UUID REFERENCES auth.users(id),
    created_at  timestamp with time zone                not null default now(),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
        
    constraint races_location_id_fkey foreign KEY (location_id) references locations (id)
);