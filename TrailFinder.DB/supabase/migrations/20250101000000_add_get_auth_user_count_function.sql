-- supabase/migrations/<timestamp>_add_get_auth_user_count_function.sql

-- Function to get the total count of all authenticated users
CREATE OR REPLACE FUNCTION public.get_auth_user_count()
RETURNS bigint AS $$
SELECT count(*) FROM auth.users;
$$ LANGUAGE sql SECURITY DEFINER;

-- Grant execution permission to authenticated users
GRANT EXECUTE ON FUNCTION public.get_auth_user_count() TO authenticated;