// TrailFinder.Website\src\components\TrailList.tsx
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
import { useTrails } from '../hooks/useTrails';
import { useMemo } from 'react';
import {type TrailFilters } from '../types/filters'; // Ensure imported
//import type { Trail } from "@trailfinder/db-types";

const getDifficultyColor = (difficulty: string) => {
    // console.log('difficulty: ', difficulty.toLowerCase()); // Remove console.log in production code
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

const formatDistance = (distanceKm: number) => { // Changed param name from distanceMeters for clarity based on data
    return `${distanceKm.toFixed(1)} km`;
};

const formatElevation = (elevation: number) => {
    return `${elevation.toFixed(0)} m`;
};

interface TrailListProps {
    filters: TrailFilters;
}

export function TrailList({ filters }: TrailListProps) {
    const { data: allTrails, isLoading, error } = useTrails();

    const filteredTrails = useMemo(() => {
        if (!allTrails) {
            return [];
        }

        let currentFiltered = [...allTrails]; // Always start with a copy

        const lowerCaseSearchTerm = filters.searchTerm.toLowerCase();

        // 1. Search Term Filter
        if (lowerCaseSearchTerm) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.name.toLowerCase().includes(lowerCaseSearchTerm) ||
                (trail.description && trail.description.toLowerCase().includes(lowerCaseSearchTerm)) ||
                (trail.location && trail.location.toLowerCase().includes(lowerCaseSearchTerm))
            );
        }

        // 2. Distance Filter
        currentFiltered = currentFiltered.filter(trail =>
            trail.distanceKm >= filters.distance.min && trail.distanceKm <= filters.distance.max
        );

        // 3. Elevation Filter
        currentFiltered = currentFiltered.filter(trail =>
            trail.elevationGainMeters >= filters.elevation.min && trail.elevationGainMeters <= filters.elevation.max
        );

        // 4. Surface Type Filter
        if (filters.surfaceTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.surfaceTypes.includes(trail.surfaceType)
            );
        }

        // 5. Difficulty Level Filter
        if (filters.difficultyLevels.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.difficultyLevels.includes(trail.difficultyLevel)
            );
        }

        // 6. Route Type Filter
        if (filters.routeTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.routeTypes.includes(trail.routeType)
            );
        }

        // 7. Terrain Type Filter
        if (filters.terrainTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.terrainTypes.includes(trail.terrainType)
            );
        }

        // 8. Region Filter
        if (filters.regions.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.regions.some(region =>
                    trail.location && trail.location.toLowerCase().includes(region.toLowerCase())
                )
            );
        }

        return currentFiltered;
    }, [
        allTrails,
        filters.searchTerm,
        filters.distance,
        filters.elevation,
        filters.surfaceTypes,
        filters.difficultyLevels,
        filters.routeTypes,
        filters.terrainTypes,
        filters.regions,
    ]);

    if (isLoading) {
        return <Text>Hleð inn hlaupaleiðum...</Text>;
    }

    if (error) {
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir</Text>;
    }

    if (!filteredTrails?.length) {
        return <Text>Engar hlaupaleiðir fundust sem passa við valdar síur.</Text>;
    }

    return (
        <SimpleGrid cols={{ base: 1, sm: 2, lg: 3 }} spacing="md">
            {filteredTrails.map((trail) => (
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
                                    {formatDistance(trail.distanceKm)}
                                </Text>
                            </Group>

                            <Group gap="xs">
                                <IconArrowUpRight size={16} style={{ opacity: 0.7 }} />
                                <Text size="sm" c="dimmed">
                                    {formatElevation(trail.elevationGainMeters || 0)}
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