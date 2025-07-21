// src/components/TrailsTable.tsx
import {Table, Text, Group} from '@mantine/core'; // Added Card, just in case
import { NavLink as MantineNavLink } from '@mantine/core';
import { NavLink as RouterNavLink } from 'react-router-dom';

import { IconActivity, IconRuler, IconMountain } from "@tabler/icons-react"; // Added Icons

import {
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation, getRouteTypeTranslation
} from '../utils/TrailUtils'; // Import necessary utility functions

interface TrailsTableProps {
    searchTerm: string; // Add searchTerm prop
}
import { useTrails } from '../hooks/useTrails'; // Now using the hook with 'allTrailsList' queryKey
import { useMemo } from 'react';

interface TrailsTableProps {
    searchTerm: string;
}

export function TrailsTable({ searchTerm }: TrailsTableProps) { // Accept searchTerm as prop
    // Fetch ALL trails from the central useTrails hook
    const { data: allTrails, isLoading, error } = useTrails(); // Destructure 'data' as 'allTrails'

    // Client-side filtering logic
    const filteredTrails = useMemo(() => {
        if (!allTrails) {
            return []; // Return empty array if data isn't loaded yet
        }

        if (!searchTerm) {
            return allTrails; // Show all if no search term
        }

        const lowerCaseSearchTerm = searchTerm.toLowerCase();
        return allTrails.filter(trail =>
            trail.name.toLowerCase().includes(lowerCaseSearchTerm) ||
            (trail.description && trail.description.toLowerCase().includes(lowerCaseSearchTerm)) ||
            (trail.location && trail.location.toLowerCase().includes(lowerCaseSearchTerm))
        );
    }, [allTrails, searchTerm]); // Recalculate only when allTrails or searchTerm changes


    if (isLoading) {
        return <Text>Hleð inn hlaupaleiðum...</Text>;
    }

    if (error) {
        // You can add more detailed error logging here if needed
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir: {error.message}</Text>;
    }

    if (!filteredTrails?.length) { // Check filteredTrails length
        return <Text>Engar hlaupaleiðir fundust sem passa við leitina.</Text>; // More specific message
    }

    const rows = filteredTrails.map((trail) => ( // Map over filteredTrails
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
            <Table.Td>{getRouteTypeTranslation(trail.routeType)}</Table.Td> {/* Make sure getRouteTypeTranslation exists */}
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