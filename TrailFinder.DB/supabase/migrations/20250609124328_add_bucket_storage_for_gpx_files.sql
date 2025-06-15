-- First, ensure the bucket exists and is public
insert into storage.buckets (id, name, public)
values ('gpx-files', 'gpx-files', true)  -- Set public to true since files should be publicly accessible
    ON CONFLICT (id) DO UPDATE SET public = true;

-- Clear any existing policies for the bucket
DROP POLICY IF EXISTS "GPX files are viewable by everyone" ON storage.objects;
DROP POLICY IF EXISTS "Users can upload GPX files" ON storage.objects;
DROP POLICY IF EXISTS "Allow system to insert GPX files" ON storage.objects;

-- Add policies for the storage bucket
CREATE POLICY "GPX files are viewable by everyone"
    ON storage.objects FOR SELECT
    USING (bucket_id = 'gpx-files');  -- Allow anyone to view files in this bucket

CREATE POLICY "Allow system to insert GPX files"
    ON storage.objects FOR INSERT
    TO postgres  -- This allows the postgres role (used in migrations) to insert files
    WITH CHECK (bucket_id = 'gpx-files');