// src/pages/TrailDetails.tsx

import { notifications } from '@mantine/notifications';
import {useNavigate, useParams} from 'react-router-dom';
import {useTrail} from '../hooks/useTrail';
import {
    ActionIcon,
    Badge,
    Box,
    Button,
    Card,
    Container,
    Divider,
    Group,
    Menu,
    Stack,
    Text,
    Title,
    Tooltip
} from '@mantine/core';
import {
    IconBrandFacebook,
    IconBrandTelegram,
    IconBrandWhatsapp,
    IconDotsVertical,
    IconDownload,
    IconLink,
    IconMapPin,
    IconMountain,
    IconQrcode,
    IconRuler,
} from '@tabler/icons-react';

import {TrailLoader} from '../components/TrailLoader';
import {useEffect, useRef} from 'react';
import {TrailNotFoundError} from "../types/api.ts";

import {
    getDifficultyLevelTranslation,
    getRouteTypeIcon,
    getRouteTypeTranslation,
    getSurfaceTypeTranslation,
    getTerrainTypeTranslation
} from '../utils/TrailUtils';
import {useUserLocation} from '../hooks/useUserLocation';
import type {TrailGpxDownloadHandle} from "../components/TrailGpxDownload.tsx";
import type { TrailQrCodeButtonHandle } from '../components/TrailQrCodeButton.tsx';
import TrailQrCodeButton from '../components/TrailQrCodeButton.tsx';
import TrailGpxDownload from '../components/TrailGpxDownload.tsx';
import {useMediaQuery} from "@mantine/hooks";
import { TrailMap2 } from '../components/TrailMap2.tsx';

export function TrailDetails() {
    const isMobile = useMediaQuery('(max-width: 768px)');
    const {slug} = useParams<{ slug: string }>();
    const navigate = useNavigate();

    const downloadButtonRef = useRef<TrailGpxDownloadHandle>(null);
    const qrCodeButtonRef = useRef<TrailQrCodeButtonHandle>(null);
    
    // Get user location
    const userLocation = useUserLocation();

    const {data: trail, isLoading, error} = useTrail(
        {
            slug: slug !== undefined ? slug : '',
            userLatitude: userLocation.latitude,
            userLongitude: userLocation.longitude,
        }
    );

    const isTrailSpecificError = error instanceof TrailNotFoundError;

    useEffect(() => {
        if (error && !isTrailSpecificError) {
            console.error("General error loading trail:", error);
        }
    }, [error, isTrailSpecificError]);

    // Simplified loading check
    if (isLoading) {
        return <TrailLoader/>;
    }

    if (isTrailSpecificError || error || !trail) {
        return (
            <Container ta="center" style={{padding: '4rem 0'}}>
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

    //TODO: reconsider:
    trail.distanceToUserKm = trail.distanceToUserKm || 0; // Set rhw default value to 0 if undefined

    const shareOptions = [
        {
            icon: IconBrandWhatsapp,
            label: 'WhatsApp',
            onClick: () => {
                const text = `Skoðaðu þessa leið á hlaupaleidir.is: ${trail.name} ${window.location.href}`;
                window.open(`https://wa.me/?text=${encodeURIComponent(text)}`, '_blank');
            }
        },
        {
            icon: IconBrandFacebook,
            label: 'Facebook',
            onClick: () => {
                const url = encodeURIComponent(window.location.href);
                const title = encodeURIComponent(`Skoðaðu þessa leið á hlaupaleidir.is: ${trail.name}`);
                window.open(`https://www.facebook.com/sharer/sharer.php?u=${url}&t=${title}`, '_blank');
            }
        },
        {
            icon: IconBrandTelegram,
            label: 'Telegram',
            onClick: () => {
                const text = `Skoðaðu þessa leið á hlaupaleidir.is: ${trail.name} ${window.location.href}`;
                window.open(`https://t.me/share/url?url=${encodeURIComponent(window.location.href)}&text=${encodeURIComponent(text)}`, '_blank');
            }
        },
        {
            icon: IconLink,
            label: 'Afrita hlekk',
            onClick: () => {
                navigator.clipboard.writeText(window.location.href)
                    .then(() => notifications.show(
                        {
                            title: 'Tókst!',
                            message: 'Hlekkurinn var afritaður',
                            color: 'green'
                        }
                    ));
            }
        }
    ];

    // Reiknum út hvort við eigum að sýna fyrirsögnina: Aðgerðir
    const shouldShowActionsLabel = trail.routeGeom != null && isMobile;

    return (
        <Container size="lg">
            <Stack gap="md">
                <Title order={1}>{trail.name}</Title>

                {/* Badges with Translations */}
                <Group gap="xs">
                    <Tooltip label="Hvernig er leiðin?" position="bottom" offset={2} arrowOffset={14} arrowSize={4}
                             withArrow>
                        <Badge color="blue">{getDifficultyLevelTranslation(trail.difficultyLevel)}</Badge></Tooltip>
                    <Tooltip label="Hvernig er landslagið?" position="bottom" offset={2} arrowOffset={14} arrowSize={4}
                             withArrow>
                        <Badge color="grape">{getTerrainTypeTranslation(trail.terrainType)}</Badge></Tooltip>
                    <Tooltip label="Malbik eða mói?" position="bottom" offset={2} arrowOffset={14} arrowSize={4}
                             withArrow>
                        <Badge color="teal">{getSurfaceTypeTranslation(trail.surfaceType)}</Badge></Tooltip>
                </Group>

                {/* Trail Overview Card */}
                <Card withBorder>
                    <Group gap="xl" wrap="wrap">
                        {/* Vegalengd (Distance) */}
                        <Group>
                            <IconRuler size={20}/>
                            <div>
                                <Text size="sm" c="dimmed">Vegalengd</Text>
                                <Text>{trail.distanceKm.toFixed(1)} km</Text>
                            </div>
                        </Group>

                        {/* Hækkun (Elevation) */}
                        <Group>
                            <IconMountain size={20}/>
                            <div>
                                <Text size="sm" c="dimmed">Hækkun</Text>
                                <Text>{trail.elevationGainMeters} m</Text>
                            </div>
                        </Group>

                        {/* Tegund leiðar (Route Type) */}
                        <Group>
                            <DynamicRouteIcon size={20}/>
                            <div>
                                <Text size="sm" c="dimmed">Tegund leiðar</Text>
                                <Text>{getRouteTypeTranslation(trail.routeType)}</Text>
                            </div>
                        </Group>

                        {/* Fjarlægð leiðar frá staðsetningu notanta */}
                        {trail.distanceToUserKm > 0 && (
                            <Group>
                                <IconMapPin size={20}/>
                                <div>
                                    <Text size="sm" c="dimmed">Fjarlægð að leið</Text>
                                    <Text>{trail.distanceToUserKm.toFixed(1)} km</Text>
                                </div>
                            </Group>
                        )}

                        {/* Menu fyrir aðgerðir */}
                        <Group ml="auto" gap="xs">
                            {trail.routeGeom != null && (
                                <>
                                    <Box visibleFrom="md">
                                        <TrailGpxDownload trail={trail} ref={downloadButtonRef} />
                                    </Box>
                                    <Box style={{ display: 'none' }}>
                                        <TrailGpxDownload trail={trail} ref={downloadButtonRef} />
                                    </Box>
                                </>
                            )}

                            {/* QR Code button */}
                            <Box visibleFrom="md">
                                <TrailQrCodeButton trail={trail} ref={qrCodeButtonRef} />
                            </Box>
                            <Box style={{ display: 'none' }}>
                                <TrailQrCodeButton trail={trail} ref={qrCodeButtonRef} />
                            </Box>

                            <Menu shadow="md" width={200}>
                                <Menu.Target>
                                    <ActionIcon
                                        variant="default"
                                        size="lg"
                                        aria-label="Aðgerðir"
                                    >
                                        <IconDotsVertical style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                    </ActionIcon>
                                </Menu.Target>

                                <Menu.Dropdown>
                                    {/* Sýnum 'Aðgerðir' merkið aðeins ef einhver atriði eru sýnileg í valmyndinni */}
                                    {shouldShowActionsLabel && (
                                        <Menu.Label>Aðgerðir</Menu.Label>
                                    )}

                                    {trail.routeGeom != null && (
                                        <Box hiddenFrom="md">
                                            <Menu.Item
                                                leftSection={<IconDownload size={14} />}
                                                onClick={() => downloadButtonRef.current?.handleDownload()}
                                            >
                                                Hlaða niður GPX
                                            </Menu.Item>
                                        </Box>
                                    )}

                                    <Box hiddenFrom="md">
                                        <Menu.Item
                                            leftSection={<IconQrcode size={14}/>}
                                            onClick={() => qrCodeButtonRef.current?.handleQrCode()}
                                            display={qrCodeButtonRef.current?.isOpened ? 'none' : undefined}
                                            hiddenFrom="md"
                                        >
                                            Sýna QR kóða
                                        </Menu.Item>
                                    </Box>

                                    {/* Share options */}
                                    <Menu.Divider />
                                    <Menu.Label>Deila</Menu.Label>
                                    {shareOptions.map((option) => (
                                        <Menu.Item
                                            key={option.label}
                                            leftSection={<option.icon size={14} />}
                                            onClick={option.onClick}
                                        >
                                            {option.label}
                                        </Menu.Item>
                                    ))}
                                </Menu.Dropdown>
                            </Menu>
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
                
                {trail.routeGeom != null && (
                    <Card withBorder>
                        <TrailMap2 
                            slug={trail.slug} 
                            userLongitude={userLocation.longitude} 
                            userLatitude={userLocation.latitude} />
                    </Card>
                )}
                
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
                <Divider my="md"/>
                <Group justify="flex-end">
                    <Text size="sm" c="dimmed">
                        Bætt við: {trailAddedDate}
                    </Text>
                </Group>
            </Stack>
        </Container>
    );
}