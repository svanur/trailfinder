-- Migration Script: Create gpx_files Table

-- 1. Create the Trigger Function
-- This function automatically sets the 'updated_at' column to the current timestamp
-- whenever the row is updated.
CREATE OR REPLACE FUNCTION set_updated_at_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- 2. Create the gpx_files Table
-- This table stores metadata about GPX files uploaded for trails,
-- with the actual file content residing in Supabase Storage.
CREATE TABLE public.gpx_files
(
    id                 UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    -- Foreign key to the trails table, enforcing a one-to-one relationship
    -- (each trail can have at most one GPX file entry).
    -- ON DELETE CASCADE ensures that if a trail is deleted, its associated
    -- GPX file metadata entry is also automatically deleted.
    trail_id           UUID UNIQUE                     NOT NULL REFERENCES public.trails (id) ON DELETE CASCADE,

    -- The full path to the GPX file within Supabase Storage (e.g., 'trails/uuid/trail-slug.gpx')
    -- Consider adding a UNIQUE constraint if you want to prevent multiple metadata entries
    -- pointing to the exact same storage path.
    storage_path       TEXT                            NOT NULL,

    -- The original filename provided by the user during upload
    original_file_name VARCHAR(255)                    NOT NULL,

    -- The sanitized filename used for storage (e.g., 'trail-slug.gpx')
    -- This stores the actual filename used in the storage bucket.
    file_name          VARCHAR(255)                    NOT NULL,

    -- The size of the GPX file in bytes
    file_size          BIGINT                          NOT NULL,

    -- The MIME type of the file (e.g., 'application/gpx+xml')
    content_type       VARCHAR(100)                    NOT NULL,

    -- Flag for soft deletion: TRUE for active, FALSE for soft-deleted.
    is_active BOOLEAN DEFAULT TRUE NOT NULL,

    -- Audit fields
    created_at         TIMESTAMPTZ      DEFAULT NOW() NOT NULL,
    -- User who uploaded the GPX file (references auth.users table)
    created_by         UUID REFERENCES auth.users (id) NOT NULL,

    -- Timestamp of the last update, managed by a database trigger.
    updated_at         TIMESTAMPTZ, 
    updated_by         UUID REFERENCES auth.users (id) -- User who last updated the metadata.
);

-- 3. Create Indexes
-- Create an index on trail_id for faster lookups when retrieving
-- GPX file information for a specific trail.
CREATE INDEX idx_gpx_files_trail_id ON public.gpx_files (trail_id);

-- 4. Create Triggers
-- This trigger ensures that 'updated_at' is automatically set
-- on every update operation for the gpx_files table.
CREATE TRIGGER set_gpx_files_updated_at
    BEFORE UPDATE ON public.gpx_files
    FOR EACH ROW
    EXECUTE FUNCTION set_updated_at_timestamp();
