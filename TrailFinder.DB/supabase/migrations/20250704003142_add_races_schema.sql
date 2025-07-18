create table public.races
(
    id                UUID PRIMARY KEY                  DEFAULT gen_random_uuid(),
    location_id       UUID null,
    name              text null,
    slug              text null,
    description       text null,
    web_url           TEXT,
    race_status       race_status not null,
    recurring_month   smallint null,
    recurring_week    smallint null,
    recurring_weekday smallint null,

    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by UUID REFERENCES auth.users(id) NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    updated_by UUID REFERENCES auth.users(id),

    constraint races_location_id_fkey foreign KEY (location_id) references locations (id)
);