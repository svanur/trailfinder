import { createClient } from '@supabase/supabase-js'

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL
const supabaseAnonKey = import.meta.env.VITE_SUPABASE_ANON_KEY

if (!supabaseUrl || !supabaseAnonKey) {
    throw new Error('Missing Supabase environment variables')
}

export const supabase = createClient(supabaseUrl, supabaseAnonKey)

export async function testConnection() {
    try {
        const { data, error } = await supabase
            .from('trails')
            .select('*')
            .limit(1)

        if (error) {
            console.error('Connection test error:', error)
            return false
        }

        console.log('Connection test successful:', data)
        return true
    } catch (err) {
        console.error('Connection test failed:', err)
        return false
    }
}
