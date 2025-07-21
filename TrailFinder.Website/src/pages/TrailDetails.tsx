// src/pages/TrailDetails.tsx
import { useParams, useNavigate } from 'react-router-dom';
import { useTrail } from '../hooks/useTrail';
import { Container, Title, Text, Group, Stack, Badge, Card, Button } from '@mantine/core';
import { IconRuler, IconMountain } from '@tabler/icons-react'; // Removed IconRoute, will be dynamic


// Import your new custom loader component
import { TrailLoader } from '../components/TrailLoader';
import { useEffect } from 'react';
import { TrailNotFound } from "./TrailNotFound.tsx";
import { TrailNotFoundError } from "../types/api.ts";

// Import the utility functions
import {
    getRouteTypeIcon,
    getRouteTypeTranslation,
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation
} from '../utils/TrailUtils'; // Adjust path if needed


export function TrailDetails() {
    const { slug } = useParams<{ slug: string }>();
    const navigate = useNavigate();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');

    const isTrailSpecificError = error instanceof TrailNotFoundError;

    useEffect(() => {
        if (error && !isTrailSpecificError) {
            console.error("General error loading trail:", error);
        }
    }, [error, isTrailSpecificError]);


    if (isLoading) {
        return <TrailLoader />;
    }

    if (isTrailSpecificError) {
        return <TrailNotFound />;
    }

    if (error) {
        return <TrailNotFound />;
    }

    if (!trail) {
        return (
            <Container ta="center" style={{ padding: '4rem 0' }}>
                <Title order={2}>Engin gögn fundust!</Title>
                <Text mt="md">Einhver villa kom upp og engin gönguleiðargögn fundust.</Text>
                <Button mt="lg" onClick={() => navigate('/')}>Aftur á forsíðu</Button>
            </Container>
        );
    }

    // Get the correct icon component based on routeType
    const DynamicRouteIcon = getRouteTypeIcon(trail.routeType);

    return (
        <Container size="lg">
            <Stack gap="md">
                <Title order={1}>{trail.name}</Title>

                <Card withBorder>
                    <Group gap="xl">
                        {/* Vegalengd (Distance) */}
                        <Group>
                            <IconRuler size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Vegalengd</Text>
                                <Text>{trail.distanceKm.toFixed(1)} km</Text>
                            </div>
                        </Group>

                        {/* Hækkun (Elevation) */}
                        <Group>
                            <IconMountain size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Hækkun</Text>
                                <Text>{trail.elevationGainMeters} m</Text>
                            </div>
                        </Group>

                        {/* Tegund leiðar (Route Type) */}
                        <Group>
                            <DynamicRouteIcon size={20} /> {/* Use the dynamic icon */}
                            <div>
                                <Text size="sm" c="dimmed">Tegund leiðar</Text>
                                <Text>{getRouteTypeTranslation(trail.routeType)}</Text> {/* Use translation */}
                            </div>
                        </Group>
                    </Group>
                </Card>

                <Card withBorder>
                    <Text size="lg" fw={500} mb="md">Um hlaupaleiðina</Text>
                    <Text>{trail.description}</Text>
                </Card>

                {/* Badges with Translations */}
                <Group gap="xs">
                    <Badge color="blue">{getDifficultyLevelTranslation(trail.difficultyLevel)}</Badge>
                    <Badge color="grape">{getTerrainTypeTranslation(trail.terrainType)}</Badge>
                    <Badge color="teal">{getSurfaceTypeTranslation(trail.surfaceType)}</Badge>
                </Group>
            </Stack>
        </Container>
    );
}