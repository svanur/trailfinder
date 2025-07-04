create table locations
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    parent_id   UUID null,    
    name        text null,
    slug        text null,
    description text null,

    latitude DOUBLE PRECISION,
    longitude DOUBLE PRECISION,
    --point_geom geometry(LINESTRINGZ, 4326),

    user_id UUID REFERENCES auth.users(id),
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
        
    constraint locations_parent_id_fkey foreign KEY (parent_id) references locations (id)
);