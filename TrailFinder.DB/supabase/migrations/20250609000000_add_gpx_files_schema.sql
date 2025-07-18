-- Migration Script: Create gpx_files Table

-- This table stores metadata about GPX files uploaded for trails,
-- with the actual file content residing in Supabase Storage.
CREATE TABLE gpx_files
(
    id                 UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    -- Foreign key to the trails table, enforcing a one-to-one relationship
    -- (each trail can have at most one GPX file entry).
    -- ON DELETE CASCADE ensures that if a trail is deleted, its associated
    -- GPX file metadata entry is also automatically deleted.
    trail_id           UUID UNIQUE                     NOT NULL REFERENCES public.trails (id) ON DELETE CASCADE,

    -- The full path to the GPX file within Supabase Storage (e.g., 'trails/uuid/trail-slug.gpx')
    storage_path       TEXT                            NOT NULL,

    -- The original filename provided by the user during upload
    original_file_name VARCHAR(255)                    NOT NULL,

    -- The sanitized filename used for storage (e.g., 'trail-slug.gpx')
    file_name          VARCHAR(255)                    NOT NULL,

    -- The size of the GPX file in bytes
    file_size          BIGINT                          NOT NULL,

    -- The MIME type of the file (e.g., 'application/gpx+xml')
    content_type       VARCHAR(100)                    NOT NULL,

    -- Timestamp when the GPX file metadata was first created/uploaded
    uploaded_at        TIMESTamptz      DEFAULT NOW(),

    -- User who uploaded the GPX file (references auth.users table)
    uploaded_by        UUID REFERENCES auth.users (id) NOT NULL,

    -- Timestamp of the last update to this GPX file metadata entry
    updated_at         TIMESTamptz      DEFAULT NOW(),

    -- User who last updated this GPX file metadata entry
    updated_by         UUID REFERENCES auth.users (id) NOT NULL
);

-- Create an index on trail_id for faster lookups when retrieving
-- GPX file information for a specific trail.
CREATE INDEX idx_gpx_files_trail_id ON gpx_files (trail_id);