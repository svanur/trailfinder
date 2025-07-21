// src/components/TrailsTable.tsx
import { Table, Text, Group, Flex } from '@mantine/core'; // Flex should now be fine
import { NavLink as MantineNavLink } from '@mantine/core';
import { NavLink as RouterNavLink } from 'react-router-dom';

import { useTrails } from '../hooks/useTrails';
import { IconActivity, IconRuler, IconMountain, IconArrowUp, IconArrowDown } from "@tabler/icons-react"; // Arrow icons should now be fine

import {
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation,
    getRouteTypeTranslation
} from '../utils/TrailUtils';
import {useMemo, useState} from 'react';
import {type TrailFilters } from '../types/filters';
import type {Trail} from "@trailfinder/db-types";

interface TrailsTableProps {
    filters: TrailFilters;
}

type SortKey = keyof Trail | null;
type SortDirection = 'asc' | 'desc';

export function TrailsTable({ filters }: TrailsTableProps) {
    const { data: allTrails, isLoading, error } = useTrails();

    const [sortKey, setSortKey] = useState<SortKey>(null);
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

    const handleSort = (key: keyof Trail) => {
        if (sortKey === key) {
            setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
        } else {
            setSortKey(key);
            setSortDirection('asc');
        }
    };

    const renderSortIcon = (key: keyof Trail) => {
        if (sortKey === key) {
            return sortDirection === 'asc'
                ? <IconArrowUp size={14} />
                : <IconArrowDown size={14} />;
        }
        return null;
    };

    const filteredAndSortedTrails = useMemo(() => { // Renamed for clarity, was 'filteredTrails'
        if (!allTrails) {
            return [];
        }

        // --- FILTERING LOGIC (keep as is) ---
        let currentFiltered = [...allTrails]; // ***CRUCIAL: Create a copy here before filtering/sorting***
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

        // --- RE-ADDING SORTING LOGIC ---
        if (sortKey) {
            currentFiltered.sort((a, b) => {
                const aValue = a[sortKey];
                const bValue = b[sortKey];

                // Handle null/undefined values for consistent sorting
                // Puts null/undefined at the end for asc, at the beginning for desc
                if (aValue === null || aValue === undefined) return sortDirection === 'asc' ? 1 : -1;
                if (bValue === null || bValue === undefined) return sortDirection === 'asc' ? -1 : 1;

                // Numeric comparison
                if (typeof aValue === 'number' && typeof bValue === 'number') {
                    return sortDirection === 'asc' ? aValue - bValue : bValue - aValue;
                }
                // String comparison (case-insensitive for names/descriptions/locations)
                if (typeof aValue === 'string' && typeof bValue === 'string') {
                    // 'is' for Icelandic collation, sensitivity:'base' for case-insensitive
                    const comparison = aValue.localeCompare(bValue, 'is', { sensitivity: 'base' });
                    return sortDirection === 'asc' ? comparison : -comparison;
                }
                // Fallback for other types or if types are mixed
                // This will use default JavaScript comparison for non-number/string types
                if (aValue < bValue) return sortDirection === 'asc' ? -1 : 1;
                if (aValue > bValue) return sortDirection === 'asc' ? 1 : -1;
                return 0; // Equal
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
        sortKey,       // Add sort dependencies
        sortDirection, // Add sort dependencies
    ]);


    if (isLoading) {
        return <Text>Hleð inn hlaupaleiðum...</Text>;
    }

    if (error) {
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir: {error.message}</Text>;
    }

    if (!filteredAndSortedTrails?.length) { // Use the new variable name
        return <Text>Engar hlaupaleiðir fundust sem passa við valdar síur.</Text>;
    }

    const rows = filteredAndSortedTrails.map((trail) => ( // Use the new variable name
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
                    <Table.Th
                        onClick={() => handleSort('name')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Nafn {renderSortIcon('name')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('distanceKm')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Vegalengd {renderSortIcon('distanceKm')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('elevationGainMeters')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Hækkun {renderSortIcon('elevationGainMeters')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('surfaceType')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Yfirborð {renderSortIcon('surfaceType')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('difficultyLevel')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Erfiðleiki {renderSortIcon('difficultyLevel')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('routeType')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Tegund {renderSortIcon('routeType')}
                        </Flex>
                    </Table.Th>
                    <Table.Th
                        onClick={() => handleSort('terrainType')}
                        style={{ cursor: 'pointer' }}
                    >
                        <Flex align="center" gap="xs">
                            Landslag {renderSortIcon('terrainType')}
                        </Flex>
                    </Table.Th>
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
            <Table.Caption>Hlaupaleiðir</Table.Caption>
        </Table>
    );
}