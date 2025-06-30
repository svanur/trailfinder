// components/TrailList.tsx
import { Card, Text, Group, Stack, Badge } from '@mantine/core';

interface Trail {
    id: string;
    name: string;
    distance: number;
    elevation: number;
    surface: string;
    region: string;
}

export function TrailList() {
    // Sýnidæmi um gögn - seinna meir muntu sækja þetta frá API
    const trails: Trail[] = [
        {
            id: '1',
            name: 'Úlfarsfell',
            distance: 5.2,
            elevation: 296,
            surface: 'Stígur',
            region: 'Höfuðborgarsvæðið'
        },
        // Fleiri leiðir hér...
    ];

    return (
        <Stack>
            {trails.map((trail) => (
                <Card key={trail.id} shadow="sm" padding="lg" radius="md" withBorder>
                    <Group justify="space-between" mb="xs">
                        <Text fw={500} size="lg">{trail.name}</Text>
                        <Badge>{trail.region}</Badge>
                    </Group>

                    <Group>
                        <Text size="sm">Vegalengd: {trail.distance} km</Text>
                        <Text size="sm">Hækkun: {trail.elevation} m</Text>
                        <Text size="sm">Undirlag: {trail.surface}</Text>
                    </Group>
                </Card>
            ))}
        </Stack>
    );
}