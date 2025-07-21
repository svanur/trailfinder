// src/pages/Home.tsx
import { useState, useMemo } from 'react';
import { Container, Title, Text, TextInput, Stack, Card, Group, Badge, Loader, Center } from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import type {Trail} from '@trailfinder/db-types/types/database'; // Adjust path if needed
// Adjust path if needed
import { useNavigate } from 'react-router-dom';
import { IconSearch, IconRuler, IconMountain } from '@tabler/icons-react';

import {
    getDifficultyLevelTranslation,
    //getTerrainTypeTranslation,
    //getSurfaceTypeTranslation
} from '../utils/TrailUtils';

export function Home() {
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState('');

    // Fetch ALL trails from the API once
    // Explicitly using 'data' from useQuery and handling its potential undefined state
    const { data, isLoading, isError, error } = useQuery<Trail[], Error>({
        queryKey: ['allTrails'],
        queryFn: async () => {
            const response = await axios.get<Trail[]>('/api/trails');
            // Your backend returns an array directly, so we just return response.data
            // Add console.log here to verify what response.data looks like at this point
            // console.log("API Response Data:", response.data);
            return response.data.sort((a, b) => a.name.localeCompare(b.name));
        },
        staleTime: 5 * 60 * 1000
        
        
    });

    // Use a variable to hold the actual array of trails, which might be undefined initially
    const allTrails = data; // 'data' from useQuery is the array (or undefined/null)

    // Client-side filtering of trails based on searchTerm
    const filteredTrails = useMemo(() => {
        // Ensure allTrails is not null or undefined before attempting to filter
        if (!allTrails) {
            return []; // Return an empty array if data isn't available yet
        }

        if (!searchTerm) {
            return allTrails; // Show all if no search term
        }

        const lowerCaseSearchTerm = searchTerm.toLowerCase();
        return allTrails.filter(trail =>
            trail.name.toLowerCase().includes(lowerCaseSearchTerm) ||
            (trail.description && trail.description.toLowerCase().includes(lowerCaseSearchTerm)) || // Check description for null/undefined
            (trail.location && trail.location.toLowerCase().includes(lowerCaseSearchTerm)) // Check location for null/undefined
        );
    }, [allTrails, searchTerm]); // Recalculate only when allTrails or searchTerm changes

    if (isError) {
        return (
            <Container ta="center" mt="xl">
                <Title order={2}>Villa kom upp!</Title>
                <Text c="dimmed">Ekki tókst að sækja hlaupaleiðir. Vinsamlegast reynið aftur síðar. Villuskilaboð: {error?.message}</Text>
            </Container>
        );
    }

    return (
        <Container size="lg" py="md">
            <Stack gap="lg">
                <Title order={1}>hlaupaleiðir</Title>

                {/* Search Input */}
                <TextInput
                    placeholder="Leita að hlaupaleiðum..."
                    value={searchTerm}
                    onChange={(event) => setSearchTerm(event.currentTarget.value)}
                    leftSection={<IconSearch size={16} />}
                    size="md"
                    radius="md"
                />

                {isLoading ? (
                    <Center style={{ height: 200 }}>
                        <Loader size="lg" />
                    </Center>
                ) : filteredTrails.length === 0 ? (
                    <Center style={{ height: 200 }}>
                        <Text c="dimmed" size="lg">Engar hlaupaleiðir fundust.</Text>
                    </Center>
                ) : (
                    <Stack gap="md">
                        {filteredTrails.map((trail) => (
                            <Card
                                key={trail.id}
                                withBorder
                                shadow="sm"
                                padding="lg"
                                radius="md"
                                onClick={() => navigate(`/trail/${trail.slug}`)}
                                style={{ cursor: 'pointer', transition: 'transform 150ms ease-in-out' }}
                                onMouseEnter={(e) => (e.currentTarget.style.transform = 'translateY(-3px)')}
                                onMouseLeave={(e) => (e.currentTarget.style.transform = 'translateY(0)')}
                            >
                                <Group justify="space-between" align="center" mb="xs">
                                    <Title order={3}>{trail.name}</Title>
                                    <Group gap="xs">
                                        <Badge color="blue">{getDifficultyLevelTranslation(trail.difficultyLevel)}</Badge>
                                    </Group>
                                </Group>
                                <Text size="sm" c="dimmed" lineClamp={2} mb="sm">
                                    {trail.description}
                                </Text>
                                <Group gap="md">
                                    <Group gap="xs">
                                        <IconRuler size={16} />
                                        <Text size="sm">{trail.distanceKm.toFixed(1)} km</Text>
                                    </Group>
                                    <Group gap="xs">
                                        <IconMountain size={16} />
                                        <Text size="sm">{trail.elevationGainMeters} m</Text>
                                    </Group>
                                    <Text size="sm" c="dimmed">
                                        {trail.location}
                                    </Text>
                                </Group>
                            </Card>
                        ))}
                    </Stack>
                )}
            </Stack>
        </Container>
    );
}
