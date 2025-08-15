// src/components/TrailCards.tsx

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
    IconMountain,
    IconMapPin // Added IconMapPin
} from '@tabler/icons-react';
import { useTrails } from '../hooks/useTrails';
import { useMemo, useEffect, useState } from 'react';
import {type TrailFilters } from '../types/filters';
import type { Trail } from "@trailfinder/db-types";

import {useUserLocation} from "../hooks/useUserLocation.ts";

const getDifficultyColor = (difficulty: string) => {
    switch (difficulty.toLowerCase()) {
        case 'easy': return 'green';
        case 'moderate': return 'yellow';
        case 'hard': return 'orange';
        case 'extreme': return 'red';
        default: return 'gray';
    }
};

const formatDistance = (distanceKm: number) => {
    return `${distanceKm.toFixed(1)} km`;
};

const formatElevation = (elevation: number) => {
    return `${elevation.toFixed(0)} m`;
};

interface TrailListProps {
    filters: TrailFilters;
}

export function TrailCards({ filters }: TrailListProps) {
    const userLocation = useUserLocation();

    const { data: allTrails, isLoading, error } = useTrails({
        userLatitude: userLocation.latitude,
        userLongitude: userLocation.longitude
    });

    // We'll manage sorting directly in this component for the cards
    const [sortKey, setSortKey] = useState<keyof Trail | null>(null);
    const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');

    // Default sort for cards: by distance if location available, otherwise by name
    useEffect(() => {
        if (!sortKey) {
            if (userLocation.latitude && userLocation.longitude) {
                setSortKey('distanceToUserKm');
                setSortDirection('asc');
            } else {
                setSortKey('name');
                setSortDirection('asc');
            }
        }
    }, [userLocation.latitude, userLocation.longitude, sortKey]);


    const filteredAndSortedTrails = useMemo(() => {
        if (!allTrails) {
            return [];
        }

        let currentFiltered = [...allTrails];

        const lowerCaseSearchTerm = filters.searchTerm.toLowerCase();

        // Filtering logic (same as TrailsTable)
        if (lowerCaseSearchTerm) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.name.toLowerCase().includes(lowerCaseSearchTerm) ||
                (trail.description && trail.description.toLowerCase().includes(lowerCaseSearchTerm)) ||
                (trail.location && trail.location.toLowerCase().includes(lowerCaseSearchTerm))
            );
        }

        currentFiltered = currentFiltered.filter(trail =>
            trail.distanceKm >= filters.distance.min && trail.distanceKm <= filters.distance.max
        );

        currentFiltered = currentFiltered.filter(trail =>
            trail.elevationGainMeters >= filters.elevation.min && trail.elevationGainMeters <= filters.elevation.max
        );

        if (filters.surfaceTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.surfaceTypes.includes(trail.surfaceType)
            );
        }

        if (filters.difficultyLevels.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.difficultyLevels.includes(trail.difficultyLevel)
            );
        }

        if (filters.routeTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.routeTypes.includes(trail.routeType)
            );
        }

        if (filters.terrainTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.terrainTypes.includes(trail.terrainType)
            );
        }

        if (filters.regions.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.regions.some(region =>
                    trail.location && trail.location.toLowerCase().includes(region.toLowerCase())
                )
            );
        }

        // Sorting logic (duplicated from TrailsTable, but necessary here too)
        if (sortKey === 'distanceToUserKm') {
            currentFiltered.sort((a, b) => {
                const aDist = a.distanceToUserKm ?? Infinity;
                const bDist = b.distanceToUserKm ?? Infinity;
                return sortDirection === 'asc' ? aDist - bDist : bDist - aDist;
            });
        } else if (sortKey) {
            currentFiltered.sort((a, b) => {
                const aValue = a[sortKey];
                const bValue = b[sortKey];

                if (aValue === null || aValue === undefined) return sortDirection === 'asc' ? 1 : -1;
                if (bValue === null || bValue === undefined) return sortDirection === 'asc' ? -1 : 1;

                if (typeof aValue === 'number' && typeof bValue === 'number') {
                    return sortDirection === 'asc' ? aValue - bValue : bValue - aValue;
                }
                if (typeof aValue === 'string' && typeof bValue === 'string') {
                    const comparison = aValue.localeCompare(bValue, 'is', { sensitivity: 'base' });
                    return sortDirection === 'asc' ? comparison : -comparison;
                }
                return 0;
            });
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
        sortKey, // Add sort dependencies
        sortDirection, // Add sort dependencies
    ]);


    //if (isLoading || userLocation.isLoading) {
    if (isLoading ) {
        return <Text>Hleð inn hlaupaleiðum {/* og staðsetningu notanda */}...</Text>;
    }

    {/* 
    if (userLocation.error) {
        return <Text c="orange">Gat ekki náð í staðsetningu notanda: {userLocation.error.message}. Hlaupaleiðir verða ekki flokkaðar eftir fjarlægð.</Text>;
    }
     */}

    if (error) {
        return <Text c="red">Villa kom upp við að sækja hlaupaleiðir</Text>;
    }

    if (!filteredAndSortedTrails?.length) {
        return <Text>Engar hlaupaleiðir fundust sem passa við valdar síur.</Text>;
    }

    return (
        <SimpleGrid cols={{ base: 1, sm: 2, lg: 3 }} spacing="md">
            {filteredAndSortedTrails.map((trail) => ( // Use filteredAndSortedTrails
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

                            {/* Display distance to user in card */}
                            {trail.distanceToUserKm !== null && trail.distanceToUserKm !== undefined && (
                                <Group gap="xs">
                                    <IconMapPin size={16} style={{ opacity: 0.7 }} />
                                    <Text size="sm" c="dimmed">
                                        {trail.distanceToUserKm.toFixed(2)} km
                                    </Text>
                                </Group>
                            )}
                        </Group>
                    </Stack>
                </Card>
            ))}
        </SimpleGrid>
    );
}