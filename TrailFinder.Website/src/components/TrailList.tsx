// components/TrailList.tsx
import { 
    Card, 
    Group, 
    Stack, 
    Text, 
    Badge, 
    SimpleGrid 
} from '@mantine/core';

import {
    IconRuler,
    IconArrowUpRight,
    IconMap,
    IconRoute,
    IconMountain
} from '@tabler/icons-react';
//import {DifficultyLevel} from "@trailfinder/db-types/database";
import { useTrails } from '../hooks/useTrails';

const getDifficultyColor = (difficulty: string) => {
    switch (difficulty.toLowerCase()) {
        case 'easy':
            return 'green';
        case 'moderate':
            return 'yellow';
        case 'hard':
            return 'orange';
        case 'extreme':
            return 'red';
        default:
            return 'gray';
    }
};

const formatDistance = (distance: number) => {
    return `${distance.toFixed(1)} km`;
};

const formatElevation = (elevation: number) => {
    return `${elevation.toFixed(0)} m`;
};

export function TrailList() {
    const { data: trails, isLoading, error } = useTrails();

    if (isLoading) {
        return <Text>Hleð inn hlaupaleiðum...</Text>;
    }

    if (error) {
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir</Text>;
    }

    if (!trails?.length) {
        return <Text>Engar hlaupaleiðir fundust</Text>;
    }

    return (
        <SimpleGrid cols={{ base: 1, sm: 2 }} spacing="md">
            {trails.map((trail) => (
                <Card key={trail.id} shadow="sm" padding="lg" radius="md" withBorder>
                    <Stack gap="xs">
                        <Text fw={500} size="lg">{trail.name}</Text>

                        <Group gap="xs">
                            <Badge
                                color={getDifficultyColor(trail.difficultyLevel.toString())}
                                leftSection={<IconMountain size={12} />}
                            >
                                {trail.difficultyLevel}
                            </Badge>
                            <Badge
                                variant="outline"
                                leftSection={<IconRoute size={12} />}
                            >
                                {trail.routeType}
                            </Badge>
                            <Badge
                                variant="outline"
                                leftSection={<IconMap size={12} />}
                            >
                                {trail.location}
                            </Badge>
                        </Group>

                        <Group gap="lg">
                            <Group gap="xs">
                                <IconRuler size={16} style={{ opacity: 0.7 }} />
                                <Text size="sm" c="dimmed">
                                    {formatDistance(trail.distance)}
                                </Text>
                            </Group>

                            <Group gap="xs">
                                <IconArrowUpRight size={16} style={{ opacity: 0.7 }} />
                                <Text size="sm" c="dimmed">
                                    {formatElevation(trail.elevationGain || 0)}
                                </Text>
                            </Group>

                            <Badge variant="light">
                                {trail.terrainType}
                            </Badge>
                        </Group>
                    </Stack>
                </Card>
            ))}
        </SimpleGrid>
    );
}