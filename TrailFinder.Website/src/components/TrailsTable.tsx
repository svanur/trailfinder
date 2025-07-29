// src/components/TrailsTable.tsx (Update this file)
import { Table, Text, Group, Flex } from '@mantine/core';
import { NavLink as MantineNavLink } from '@mantine/core';
import { NavLink as RouterNavLink } from 'react-router-dom';

// Import the new useUserLocation hook
import { useUserLocation } from '../hooks/useUserLocation';

import { IconActivity, IconRuler, IconMountain, IconArrowUp, IconArrowDown, IconMapPin } from "@tabler/icons-react";

import {
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation,
    getRouteTypeTranslation
} from '../utils/TrailUtils';
import {useMemo, useState, useEffect} from 'react'; // Import useEffect
import {type TrailFilters } from '../types/filters';
import {useTrails} from "../hooks/useTrails.ts";
import type {Trail} from "@trailfinder/db-types";

interface TrailsTableProps {
    filters: TrailFilters;
}

// We'll sort by distanceToUserKm, name, distanceKm, etc.
type SortKey = keyof Trail | 'distanceToUserKm' | null; // Allow sorting by new distance field
type SortDirection = 'asc' | 'desc';

export function TrailsTable({ filters }: TrailsTableProps) {
    // Get user location
    const userLocation = useUserLocation();
    console.log('userLocation', userLocation);

    // Pass user location to useTrails hook
    const { data: allTrails, isLoading, error } = useTrails({
        userLatitude: userLocation.latitude,
        userLongitude: userLocation.longitude,
    });

    const [sortKey, setSortKey] = useState<SortKey>(null);
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

    // Default sort to distance if location available, otherwise by name
    useEffect(() => {
        if (userLocation.latitude && userLocation.longitude && !sortKey) {
            setSortKey('distanceToUserKm');
            setSortDirection('asc');
        } else if (!userLocation.latitude && !userLocation.longitude && !sortKey) {
            setSortKey('name'); // Default back to name if location not available
            setSortDirection('asc');
        }
    }, [userLocation.latitude, userLocation.longitude, sortKey]);


    const handleSort = (key: SortKey) => { // Use SortKey type
        if (sortKey === key) {
            setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
        } else {
            setSortKey(key);
            setSortDirection('asc');
        }
    };

    const renderSortIcon = (key: SortKey) => { // Use SortKey type
        if (sortKey === key) {
            return sortDirection === 'asc'
                ? <IconArrowUp size={14} />
                : <IconArrowDown size={14} />;
        }
        return null;
    };

    const filteredAndSortedTrails = useMemo(() => {
        if (!allTrails) {
            return [];
        }

        let currentFiltered = [...allTrails];

        // --- FILTERING LOGIC ---
        const lowerCaseSearchTerm = filters.searchTerm.toLowerCase();

        // 1. Search Term Filter
        if (lowerCaseSearchTerm) {
            currentFiltered = currentFiltered.filter(trail =>
                    trail.name.toLowerCase().includes(lowerCaseSearchTerm) ||
                    (trail.description && trail.description.toLowerCase().includes(lowerCaseSearchTerm))
            );
        }

        // 2. Distance Filter (using DistanceKm from Trail)
        currentFiltered = currentFiltered.filter(trail =>
            trail.distanceKm !== null &&
            trail.distanceKm >= filters.distance.min && trail.distanceKm <= filters.distance.max
        );

        // 3. Elevation Filter (using ElevationGainMeters from Trail)
        currentFiltered = currentFiltered.filter(trail =>
            trail.elevationGainMeters !== null &&
            trail.elevationGainMeters >= filters.elevation.min && trail.elevationGainMeters <= filters.elevation.max
        );

        // 4. Surface Type Filter
        if (filters.surfaceTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.surfaceType !== null && filters.surfaceTypes.includes(trail.surfaceType)
            );
        }

        // 5. Difficulty Level Filter
        if (filters.difficultyLevels.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.difficultyLevel !== null && filters.difficultyLevels.includes(trail.difficultyLevel)
            );
        }

        // 6. Route Type Filter
        if (filters.routeTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.routeType !== null && filters.routeTypes.includes(trail.routeType)
            );
        }

        // 7. Terrain Type Filter
        if (filters.terrainTypes.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                trail.terrainType !== null && filters.terrainTypes.includes(trail.terrainType)
            );
        }

        // 8. Region Filter - This part needs attention if 'location' is not directly in Trail
        // You'll need to decide how regions are represented in Trail if not directly from a 'location' string.
        // For now, I'll comment it out or you'll need to adapt it based on TrailLocationDto.
        /*
        if (filters.regions.length > 0) {
            currentFiltered = currentFiltered.filter(trail =>
                filters.regions.some(region =>
                    trail.TrailLocations?.some(loc => loc.name.toLowerCase().includes(region.toLowerCase()))
                )
            );
        }
        */

        // --- SORTING LOGIC ---
        // If sorting by distance to user, ensure those with null distance are last
        if (sortKey === 'distanceToUserKm') {
            currentFiltered.sort((a, b) => {
                const aDist = a.distanceToUserKm ?? Infinity; // Treat null as very far
                const bDist = b.distanceToUserKm ?? Infinity; // Treat null as very far
                return sortDirection === 'asc' ? aDist - bDist : bDist - aDist;
            });
        }
        else if (sortKey) {
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
        filters.regions, // Keep for now, but adjust filtering logic
        sortKey,
        sortDirection,
    ]);


    // Loading states based on both trail data and user location
    if (isLoading || userLocation.isLoading) {
        return <Text>Hleð inn hlaupaleiðum og staðsetningu notanda...</Text>;
    }

    if (error) {
        return <Text c="red">Villa kom upp við að sækja hlaupaleiðir: {error.message}</Text>;
    }

    if (userLocation.error) {
        // Inform user if geolocation failed
        return <Text c="orange">Gat ekki náð í staðsetningu notanda: {userLocation.error.message}. Hlaupaleiðir verða ekki flokkaðar eftir fjarlægð.</Text>;
    }

    if (!filteredAndSortedTrails?.length) {
        return <Text>Engar hlaupaleiðir fundust sem passa við valdar síur.</Text>;
    }

    const rows = filteredAndSortedTrails.map((trail) => (
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
            {/* Display distance to user if available */}
            <Table.Td>
                <Group gap="xs">
                    <IconRuler size={16} />
                    <Text size="sm">{trail.distanceKm?.toFixed(1) ?? 'N/A'} km</Text>
                </Group>
            </Table.Td>
            <Table.Td>
                <Group gap="xs">
                    <IconMountain size={16} />
                    <Text size="sm">{trail.elevationGainMeters ?? 'N/A'} m</Text>
                </Group>
            </Table.Td>
            <Table.Td>{getSurfaceTypeTranslation(trail.surfaceType)}</Table.Td>
            <Table.Td>{getDifficultyLevelTranslation(trail.difficultyLevel)}</Table.Td>
            <Table.Td>{getRouteTypeTranslation(trail.routeType)}</Table.Td>
            <Table.Td>{getTerrainTypeTranslation(trail.terrainType)}</Table.Td>
            {/* New column for distance to user */}
            <Table.Td>
                {trail.distanceToUserKm !== null && trail.distanceToUserKm !== undefined ? (
                    <Group gap="xs">
                        <IconMapPin size={16} />
                        <Text size="sm">{trail.distanceToUserKm.toFixed(2)} km</Text>
                    </Group>
                ) : (
                    <Text size="sm" c="dimmed">Ófáanlegt</Text>
                )}
            </Table.Td>
        </Table.Tr>
    ));

    return (
        <Table stickyHeader striped highlightOnHover withTableBorder withColumnBorders stickyHeaderOffset={60}>
            <Table.Thead>
                <Table.Tr>
                    <Table.Th onClick={() => handleSort('name')} style={{ cursor: 'pointer' }}>
                        <Flex align="center" gap="xs">
                            Nafn {renderSortIcon('name')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('distanceKm')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Vegalengd {renderSortIcon('distanceKm')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('elevationGainMeters')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Hækkun {renderSortIcon('elevationGainMeters')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('surfaceType')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Yfirborð {renderSortIcon('surfaceType')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('difficultyLevel')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Erfiðleiki {renderSortIcon('difficultyLevel')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('routeType')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Tegund {renderSortIcon('routeType')}
                        </Flex>
                    </Table.Th>
                    <Table.Th onClick={() => handleSort('terrainType')} style={{ cursor: 'pointer' }}> {/* Use uppercase for DTO prop */}
                        <Flex align="center" gap="xs">
                            Landslag {renderSortIcon('terrainType')}
                        </Flex>
                    </Table.Th>
                    {/* New sortable header for distance to user */}
                    <Table.Th onClick={() => handleSort('distanceToUserKm')} style={{ cursor: 'pointer' }}>
                        <Flex align="center" gap="xs">
                            Fjarlægð {renderSortIcon('distanceToUserKm')}
                        </Flex>
                    </Table.Th>
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
            <Table.Caption>Hlaupaleiðir</Table.Caption>
        </Table>
    );
}