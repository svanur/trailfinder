// src/pages/TrailDetails.tsx
import { useParams, useNavigate } from 'react-router-dom';
import { useTrail } from '../hooks/useTrail';
// Removed: useSwipeable, useTrailsList

import { Container, Title, Text, Group, Stack, Badge, Tooltip, Card, Button, Divider
    //, Menu, ActionIcon 
} from '@mantine/core';
import {
    IconRuler, IconMountain, IconMapPin
    //, IconQrcode, IconShare, IconBrandFacebook, IconBrandTwitter, IconDownload, IconSun 
} from '@tabler/icons-react';

// Import your new custom loader component
import { TrailLoader } from '../components/TrailLoader';
import { useEffect } from 'react';
//import { TrailNotFound } from "./TrailNotFound.tsx";
import { TrailNotFoundError } from "../types/api.ts";

// Import the utility functions
import {
    getRouteTypeIcon,
    getRouteTypeTranslation,
    getDifficultyLevelTranslation,
    getTerrainTypeTranslation,
    getSurfaceTypeTranslation
} from '../utils/TrailUtils';

export function TrailDetails() {
    const { slug } = useParams<{ slug: string }>();
    const navigate = useNavigate();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');

    // Removed: useTrailsList and its related variables (isListLoading, previousTrailSlug, nextTrailSlug)

    const isTrailSpecificError = error instanceof TrailNotFoundError;

    useEffect(() => {
        if (error && !isTrailSpecificError) {
            console.error("General error loading trail:", error);
        }
    }, [error, isTrailSpecificError]);

    // Simplified loading check
    if (isLoading) {
        return <TrailLoader />;
    }

    if (isTrailSpecificError || error || !trail) {
        return (
            <Container ta="center" style={{ padding: '4rem 0' }}>
                <Title order={2}>Engin gögn fundust!</Title>
                <Text mt="md">Einhver villa kom upp og engar leiðir fundust eða slóðin er bara ekki til.</Text>
                <Button mt="lg" onClick={() => navigate('/')}>Aftur á forsíðu</Button>
            </Container>
        );
    }

    const trailCreatedAt = new Date(trail.createdAt);
    const options: Intl.DateTimeFormatOptions = { // Explicitly define type for better type-checking
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric',
        hour12: false,
        timeZone: 'Atlantic/Reykjavik'
    };
    // Create a DateTimeFormat object for Icelandic locale
    const formatter = new Intl.DateTimeFormat('is-IS', options);
    const trailAddedDate = formatter.format(trailCreatedAt);

    const DynamicRouteIcon = getRouteTypeIcon(trail.routeType);

    // Placeholder for user's current location and distance to trail
    const userDistanceToTrail = 42.0; //TODO: Implement actual user location logic later

    // TODO: Implement actual QR code generation logic later
    //const handleGenerateQrCode = () => { /* ... */ };
    // TODO: Implement actual sharing logic later
    //const handleShareFacebook = () => { /* ... */ };
    //const handleShareTwitter = () => { /* ... */ };
    
    // TODO: Implement actual GPX download logic later
    //const handleDownloadGpx = () => { /* ... */ };


    return (
        <Container size="lg">
            {/* Removed: div with {...handlers} and overflowX: 'hidden' */}
            <Stack gap="md">
                <Title order={1}>{trail.name}</Title>

                {/* Badges with Translations */}
                <Group gap="xs">
                    <Tooltip label="Hvernig er leiðin?" position="bottom" offset={2} arrowOffset={14} arrowSize={4} withArrow>
                        <Badge color="blue">{getDifficultyLevelTranslation(trail.difficultyLevel)}</Badge></Tooltip>
                    <Tooltip label="Hvernig er landslagið?" position="bottom" offset={2} arrowOffset={14} arrowSize={4} withArrow>
                        <Badge color="grape">{getTerrainTypeTranslation(trail.terrainType)}</Badge></Tooltip>
                    <Tooltip label="Malbik eða mói?" position="bottom" offset={2} arrowOffset={14} arrowSize={4} withArrow>
                        <Badge color="teal">{getSurfaceTypeTranslation(trail.surfaceType)}</Badge></Tooltip>
                </Group>

                {/* Trail Overview Card - Now with QR, Share, and GPX icons */}
                <Card withBorder>
                    <Group gap="xl" wrap="wrap">
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
                            <DynamicRouteIcon size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Tegund leiðar</Text>
                                <Text>{getRouteTypeTranslation(trail.routeType)}</Text>
                            </div>
                        </Group>

                        {/* Fjarlægð frá þér (Distance from you) */}
                        {userDistanceToTrail > 0 && (
                            <Group>
                                <IconMapPin size={20} />
                                <div>
                                    <Text size="sm" c="dimmed">Fjarlægð að leið</Text>
                                    <Text>{userDistanceToTrail.toFixed(1)} km</Text>
                                </div>
                            </Group>
                        )}

                        {/* QR Code, Social Sharing, and GPX Icons - Pushed to the right */}
                        <Group ml="auto" gap="xs">

                            {/* GPX Download Icon (Conditional) 
                            {trail.routeGeom != null && (
                                <ActionIcon
                                    variant="default"
                                    size="lg"
                                    radius="md"
                                    aria-label="Sækja GPX skrá"
                                    onClick={handleDownloadGpx}
                                >
                                    <IconDownload style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                </ActionIcon>
                            )}
                            */}

                            {/* QR Code Icon 
                            <ActionIcon
                                variant="default"
                                size="lg"
                                radius="md"
                                aria-label="Skanna QR kóða"
                                onClick={handleGenerateQrCode}
                            >
                                <IconQrcode style={{ width: '70%', height: '70%' }} stroke={1.5} />
                            </ActionIcon>
                            */}

                            {/* Share Menu Icon 
                            <Menu shadow="md" width={200}>
                                <Menu.Target>
                                    <ActionIcon
                                        variant="default"
                                        size="lg"
                                        radius="md"
                                        aria-label="Deila leiðinni"
                                    >
                                        <IconShare style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                    </ActionIcon>
                                </Menu.Target>

                                <Menu.Dropdown>
                                    <Menu.Label>Deila</Menu.Label>
                                    <Menu.Item leftSection={<IconBrandFacebook size={14} />} onClick={handleShareFacebook}>
                                        Facebook
                                    </Menu.Item>
                                    <Menu.Item leftSection={<IconBrandTwitter size={14} />} onClick={handleShareTwitter}>
                                        Twitter
                                    </Menu.Item>
                                </Menu.Dropdown>
                            </Menu>
                            */}
                            
                        </Group>
                    </Group>
                </Card>

                {/* Description Card */}
                {trail.description !== "" && (
                <Card withBorder>
                    <Text size="lg" fw={500} mb="md">Um hlaupaleiðina</Text>
                    <Text>{trail.description}</Text>
                </Card>
                )}

                {/* Elevation Graph Placeholder */}
                <Card withBorder>
                    <Text size="lg" fw={500} mb="md">Hæðarprófíll</Text>
                    <div style={{ height: 200, backgroundColor: '#f0f0f0', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: 4 }}>
                        <Text c="dimmed">Hér kemur hæðarrit (Elevation Graph)</Text>
                    </div>
                </Card>

                {/* Route Map Placeholder */}
                <Card withBorder>
                    <Text size="lg" fw={500} mb="md">Kort af leiðinni</Text>
                    <div style={{ height: 300, backgroundColor: '#e0e0e0', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: 4 }}>
                        <Text c="dimmed">Hér kemur kort af leiðinni (Route Map)</Text>
                    </div>
                </Card>

                {/* Location Details Card 
                <Card withBorder>
                    <Group mb="md">
                        <IconMapPin size={24} />
                        <Text size="lg" fw={500}>Staðsetning</Text>
                    </Group>
                    <Text>{trail.location}</Text>
                </Card>
                */}

                {/* Weather Info Placeholder 
                <Card withBorder>
                    <Group mb="md">
                        <IconSun size={24} />
                        <Text size="lg" fw={500}>Veðurspá</Text>
                    </Group>
                    <div style={{ height: 100, backgroundColor: '#f0f8ff', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: 4 }}>
                        <Text c="dimmed">Hér kemur veðurspá (Weather Forecast)</Text>
                    </div>
                </Card>
                */}

                {/* Created At - Bottom Right */}
                <Divider my="md" />
                <Group justify="flex-end">
                    <Text size="sm" c="dimmed">
                        Bætt við: {trailAddedDate}
                    </Text>
                </Group>
            </Stack>
        </Container>
    );
}