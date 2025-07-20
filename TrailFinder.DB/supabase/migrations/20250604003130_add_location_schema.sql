create table public.locations
(
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    parent_id   UUID null,
    name        text null,
    slug        text null,
    description text null,

    latitude    DOUBLE PRECISION,
    longitude   DOUBLE PRECISION,
    --point_geom geometry(LINESTRINGZ, 4326),

    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by UUID REFERENCES auth.users(id) NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    updated_by UUID REFERENCES auth.users(id),

    constraint locations_parent_id_fkey foreign KEY (parent_id) references locations (id),

    -- If you want to ensure a location can only be a child of one a parent, 
    -- or have specific hierarchical rules
    constraint unique_parent_child UNIQUE (parent_id, id)

);