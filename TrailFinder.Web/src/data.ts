import { Trail } from '@trailfinder/db-types/database';

export const trails: Trail[] = [
    {
        id: '1',
        name: 'Esja - Upp að Steini',
        slug: 'esja-upp-ad-steini',
        description: 'A popular hiking trail up Mount Esja to the Stone (Steinn). This challenging trail offers stunning views over Reykjavík and Faxaflói Bay. The path is well-marked but steep in places.',
        distance_meters: 4500,
        elevation_gain_meters: 600,
        difficulty_level: 'Moderate',
        route_geom: '',
        start_point: '',
        start_point_latitude: 64.2406,
        start_point_longitude: -21.6926,
        web_url: '',
        gpx_file_path: '',
        updated_at: '2024-01-01T00:00:00Z',
        created_at: '2024-01-01T00:00:00Z',
        user_id: '1'
    },
    {
        id: '2',
        name: 'Úlfarsfell',
        slug: 'ulfarsfell',
        description: 'A scenic loop trail to the summit of Úlfarsfell mountain, offering panoramic views of Reykjavík, Mount Esja, and on clear days, the Snæfellsnes peninsula.',
        distance_meters: 5200,
        elevation_gain_meters: 295,
        difficulty_level: 'Moderate',
        route_geom: '',
        start_point: '',
        start_point_latitude: 64.1167,
        start_point_longitude: -21.7500,
        web_url: 'https://connect.garmin.com/modern/course/60180539',
        gpx_file_path: '',
        updated_at: '2024-01-01T00:00:00Z',
        created_at: '2024-01-01T00:00:00Z',
        user_id: '1'
    },
    {
        id: '3',
        name: 'Helgafell í Mosó',
        slug: 'helgafell-mos',
        description: 'A short but rewarding hike up Helgafell mountain in Mosfellsbær. The trail offers excellent views of the surrounding area and is perfect for a quick afternoon hike.',
        distance_meters: 3000,
        elevation_gain_meters: 245,
        difficulty_level: 'Moderate',
        route_geom: '',
        start_point: '',
        start_point_latitude: 64.1670,
        start_point_longitude: -21.7144,
        web_url: 'https://connect.garmin.com/modern/course/60180539',
        gpx_file_path: '',
        updated_at: '2024-01-01T00:00:00Z',
        created_at: '2024-01-01T00:00:00Z',
        user_id: '1'
    }
];