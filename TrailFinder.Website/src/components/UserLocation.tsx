// TrailFinder.Website\src\components\UserLocation.tsx
import { useState, useEffect, useCallback } from 'react';
import {Group, Text, Button, ActionIcon, Loader, Tooltip } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import {IconMapPin, IconRefresh, IconMapPinOff, IconQuestionMark } from '@tabler/icons-react';
import { LocationPermissionModal } from './LocationPermissionModal'; // Import the new component

interface UserLocationProps {
    onLocationDetected?: (location: { lat: number; lng: number }) => void;
}

export function UserLocation({ onLocationDetected }: UserLocationProps) {
    const [userLocation, setUserLocation] = useState<{ lat: number; lng: number } | null>(null);
    const [locationName, setLocationName] = useState<string | null>(null);
    const [locationError, setLocationError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [isPermissionDenied, setIsPermissionDenied] = useState(false);

    // Mantine hook for controlling modal visibility
    const [opened, { open, close }] = useDisclosure(false);

    // Function to get user's geographic location
    const getUserGeoLocation = useCallback(() => {
        setIsLoading(true);
        setLocationError(null);
        setLocationName(null);
        setIsPermissionDenied(false);

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                async (position) => {
                    const { latitude, longitude } = position.coords;
                    setUserLocation({ lat: latitude, lng: longitude });
                    if (onLocationDetected) {
                        onLocationDetected({ lat: latitude, lng: longitude });
                    }

                    // Attempt reverse geocoding
                    try {
                        const response = await fetch(
                            `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${latitude}&lon=${longitude}`
                        );
                        const data = await response.json();
                        const city = data.address.city || data.address.town || data.address.village || data.address.suburb;
                        if (city) {
                            setLocationName(city);
                        } else {
                            setLocationName(data.display_name.split(',')[0]);
                        }
                    } catch (reverseGeocodeError) {
                        console.error("Reverse geocoding failed:", reverseGeocodeError);
                        setLocationName("Óþekktur staður");
                    } finally {
                        setIsLoading(false);
                    }
                },
                (error) => {
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            setLocationError("Leyfi hafnað");
                            setIsPermissionDenied(true);
                            break;
                        case error.POSITION_UNAVAILABLE:
                            setLocationError("Óaðgengilegt");
                            break;
                        case error.TIMEOUT:
                            setLocationError("Tímalokun");
                            break;
                        default:
                            setLocationError("Villa");
                            break;
                    }
                    setIsLoading(false);
                },
                {
                    enableHighAccuracy: true,
                    timeout: 5000,
                    maximumAge: 0,
                }
            );
        } else {
            setLocationError("Ekki stutt");
            setIsLoading(false);
        }
    }, [onLocationDetected]);

    useEffect(() => {
        getUserGeoLocation();
    }, [getUserGeoLocation]);

    return (
        <Group gap="xs" wrap="nowrap">
            {isLoading ? (
                <Group gap="xs">
                    <Loader size="xs" />
                    <Text size="sm" c="dimmed">Sæki staðsetningu...</Text>
                </Group>
            ) : locationName ? (
                <Tooltip label={`Breiddargráða: ${userLocation?.lat?.toFixed(4)}, Lengdargráða: ${userLocation?.lng?.toFixed(4)}`} withArrow>
                    <Group gap={4} wrap="nowrap" style={{cursor: 'help'}}>
                        <IconMapPin size={16} stroke={1.5} color="var(--mantine-color-blue-6)" />
                        <Text size="sm" c="dimmed">{locationName}</Text>
                    </Group>
                </Tooltip>
            ) : locationError && (
                <Group gap="xs" wrap="nowrap">
                    <Tooltip label={`${locationError}.`} withArrow>
                        <IconMapPinOff size={14} color="red" />
                    </Tooltip>

                    {!isLoading && locationError && isPermissionDenied && (
                        <Tooltip label="Reyna aftur að sækja staðsetningu" withArrow>
                            <ActionIcon variant="subtle" onClick={getUserGeoLocation} size="sm">
                                <IconRefresh size={16} stroke={1.5} />
                            </ActionIcon>
                        </Tooltip>
                    )}

                    {isPermissionDenied && (
                        <Tooltip label="Af hverju biðjum við um staðsetningu?" withArrow>
                            <Button
                                variant="subtle"
                                size="compact-xs"
                                rightSection={<IconQuestionMark size={14} />}
                                onClick={open}
                            >Hví</Button>
                        </Tooltip>
                    )}
                </Group>
            )}

            {/* Render the separate modal component */}
            <LocationPermissionModal opened={opened} onClose={close} />
        </Group>
    );
}
