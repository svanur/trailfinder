// src/components/TrailsTable.tsx
import { Table, Text, Group } from '@mantine/core';
import { NavLink as MantineNavLink } from '@mantine/core';
import { NavLink as RouterNavLink } from 'react-router-dom';

import { useTrails } from '../hooks/useTrails';
import { IconActivity, IconRuler, IconMountain } from "@tabler/icons-react";

import {
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation,
    getRouteTypeTranslation // Make sure this is imported if used
} from '../utils/TrailUtils'; // Ensure these utility functions exist
import { useMemo } from 'react';
import {type TrailFilters } from '../types/filters'; // Import filter types
//import { Trail } from '@trailfinder/db-types'; // Import Trail type

interface TrailsTableProps {
    filters: TrailFilters; // Accept the filters object as prop
}

export function TrailsTable({ filters }: TrailsTableProps) {
    const { data: allTrails, isLoading, error } = useTrails();

    const filteredTrails = useMemo(() => {
        if (!allTrails) {
            return [];
        }

        let currentFiltered = allTrails;
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

        // 8. Region Filter (assuming 'location' field can map to regions)
        // This mapping might need adjustment based on your actual data structure for 'location'
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
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir: {error.message}</Text>;
    }

    if (!filteredTrails?.length) {
        // More specific message if no trails match filters
        return <Text>Engar hlaupaleiðir fundust sem passa við valdar síur.</Text>;
    }

    const rows = filteredTrails.map((trail) => (
        <Table.Tr key={trail.id}>
            <Table.Td>
                <MantineNavLink
                    component={RouterNavLink}
                    to={`/hlaup/${trail.slug}`}
                    label={trail.name}
                    description={trail.description}
                    leftSection={<IconActivity size={16} stroke={1.5} />}
                />
            </Table.Td>
            <Table.Td>
                <Group gap="xs">
                    <IconRuler size={16} />
                    <Text size="sm">{trail.distanceKm.toFixed(1)} km</Text>
                </Group>
            </Table.Td>
            <Table.Td>
                <Group gap="xs">
                    <IconMountain size={16} />
                    <Text size="sm">{trail.elevationGainMeters} m</Text>
                </Group>
            </Table.Td>
            <Table.Td>{getSurfaceTypeTranslation(trail.surfaceType)}</Table.Td>
            <Table.Td>{getDifficultyLevelTranslation(trail.difficultyLevel)}</Table.Td>
            <Table.Td>{getRouteTypeTranslation(trail.routeType)}</Table.Td>
            <Table.Td>{getTerrainTypeTranslation(trail.terrainType)}</Table.Td>
        </Table.Tr>
    ));

    return (
        <Table stickyHeader striped highlightOnHover withTableBorder withColumnBorders stickyHeaderOffset={60}>
            <Table.Thead>
                <Table.Tr>
                    <Table.Th>Nafn</Table.Th>
                    <Table.Th>Vegalengd</Table.Th>
                    <Table.Th>Hækkun</Table.Th>
                    <Table.Th>Yfirborð</Table.Th>
                    <Table.Th>Erfiðleiki</Table.Th>
                    <Table.Th>Tegund</Table.Th>
                    <Table.Th>Landslag</Table.Th>
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
            <Table.Caption>Hlaupaleiðir</Table.Caption>
        </Table>
    );
}