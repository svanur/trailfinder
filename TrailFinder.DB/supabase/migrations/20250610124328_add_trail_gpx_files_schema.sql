-- Create the trail_gpx_files table
CREATE TABLE public.trail_gpx_files
(
    id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    trail_id     UUID REFERENCES trails (id) NOT NULL,
    file_name    TEXT                        NOT NULL,
    display_name TEXT                        NOT NULL,
    created_at   TIMESTAMPTZ      DEFAULT NOW(),
    updated_at   TIMESTAMPTZ      DEFAULT NOW()
);

-- Create updated_at trigger
CREATE TRIGGER update_trail_gpx_files_updated_at
    BEFORE UPDATE
    ON public.trail_gpx_files
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Enable RLS
ALTER TABLE public.trail_gpx_files ENABLE ROW LEVEL SECURITY;

-- Create RLS policies
CREATE
POLICY "trail_gpx_files viewable by everyone"
    ON public.trail_gpx_files FOR
SELECT
    USING (true);

CREATE
POLICY "trail_gpx_files insertable by system"
    ON public.trail_gpx_files FOR INSERT
    TO postgres
    WITH CHECK (true);

-- Create an index on trail_id for faster lookups
CREATE INDEX idx_trail_gpx_files_trail_id ON public.trail_gpx_files (trail_id);
