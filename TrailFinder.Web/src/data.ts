import { Trail } from './types';

export const trails: Trail[] = [
    {
        id: '1',
        name: 'Esja - Upp að Steini',
        normalizedName: 'esja-upp-ad-steini',
        description: 'A popular hiking trail up Mount Esja to the Stone (Steinn). This challenging trail offers stunning views over Reykjavík and Faxaflói Bay. The path is well-marked but steep in places.',
        distanceKm: 4.5,
        elevationGainMeters: 600,
        startLatitude: 64.2406,
        startLongitude: -21.6926,
        createdAt: '2024-01-01T00:00:00Z'
    },
    {
        id: '2',
        name: 'Úlfarsfell',
        normalizedName: 'ulfarsfell',
        description: 'A scenic loop trail to the summit of Úlfarsfell mountain, offering panoramic views of Reykjavík, Mount Esja, and on clear days, the Snæfellsnes peninsula.',
        distanceKm: 5.2,
        elevationGainMeters: 295,
        startLatitude: 64.1167,
        startLongitude: -21.7500,
        createdAt: '2024-01-01T00:00:00Z'
    },
    {
        id: '3',
        name: 'Helgafell í Mosó',
        normalizedName: 'helgafell-mos',
        description: 'A short but rewarding hike up Helgafell mountain in Mosfellsbær. The trail offers excellent views of the surrounding area and is perfect for a quick afternoon hike.',
        distanceKm: 3.0,
        elevationGainMeters: 245,
        startLatitude: 64.1670,
        startLongitude: -21.7144,
        createdAt: '2024-01-01T00:00:00Z'
    }
];