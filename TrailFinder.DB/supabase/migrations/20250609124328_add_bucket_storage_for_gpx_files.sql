-- First, ensure the bucket exists and is public
INSERT INTO storage.buckets (id, name, public)
VALUES ('gpx-files', 'gpx-files', true)
    ON CONFLICT (id) DO UPDATE SET public = true;

-- Clear any existing policies
DROP POLICY IF EXISTS "GPX files are viewable by everyone" 
     ON storage.objects;
     
DROP POLICY IF EXISTS "GPX files can be uploaded by authenticated users" 
     ON storage.objects;

-- Create policies for public read access
CREATE POLICY "GPX files are viewable by everyone"
    ON storage.objects FOR SELECT
    USING (bucket_id = 'gpx-files');  -- Allow anyone to view files in this bucket

-- Create policy for upload access
CREATE POLICY "GPX files can be uploaded by authenticated users"
    ON storage.objects FOR INSERT 
    WITH CHECK (bucket_id = 'gpx-files');