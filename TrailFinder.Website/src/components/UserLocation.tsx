// TrailFinder.Website\src\components\UserLocation.tsx
import { useState, useEffect, useCallback } from 'react';
import { Group, Text, ActionIcon, Loader, Tooltip } from '@mantine/core';
import { IconMapPin, IconAlertCircle, IconRefresh } from '@tabler/icons-react';

interface UserLocationProps {
    // You can pass a callback if parent needs lat/lng
    onLocationDetected?: (location: { lat: number; lng: number }) => void;
}

export function UserLocation({ onLocationDetected }: UserLocationProps) {
    const [userLocation, setUserLocation] = useState<{ lat: number; lng: number } | null>(null);
    const [locationName, setLocationName] = useState<string | null>(null);
    const [locationError, setLocationError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    // Function to get user's geographic location
    const getUserGeoLocation = useCallback(() => {
        setIsLoading(true);
        setLocationError(null);
        setLocationName(null); // Clear previous name

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
                        // Prioritize city, then town, then village, then suburb, otherwise fallback to display_name
                        const city = data.address.city || data.address.town || data.address.village || data.address.suburb;
                        if (city) {
                            setLocationName(city);
                        } else {
                            setLocationName(data.display_name.split(',')[0]); // Take first part of display name
                        }
                    } catch (reverseGeocodeError) {
                        console.error("Reverse geocoding failed:", reverseGeocodeError);
                        setLocationName("Óþekktur staður"); // Fallback if reverse geocoding fails
                    } finally {
                        setIsLoading(false);
                    }
                },
                (error) => {
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            setLocationError("Leyfi hafnað"); // Short and sweet for display
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
            setLocationError("Enginn stuðningur"); // Geolocation not supported
            setIsLoading(false);
        }
    }, [onLocationDetected]);

    // Request location on component mount
    useEffect(() => {
        getUserGeoLocation();
    }, [getUserGeoLocation]); // Dependency array includes getUserGeoLocation to prevent stale closures

    // Render logic for the location display
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
                        <Text size="sm" c="dimmed">Nálægt {locationName}</Text>
                    </Group>
                </Tooltip>
                ) : locationError ? (
                <Tooltip label={`Villa: ${locationError}. Smelltu til að reyna aftur.`} withArrow>
            <ActionIcon variant="transparent" color="red" onClick={getUserGeoLocation} size="sm">
                <IconAlertCircle size={16} stroke={1.5} />
            </ActionIcon>
        </Tooltip>
    ) : null}

{!isLoading && locationError && (
        <Tooltip label="Reyna aftur að sækja staðsetningu" withArrow>
            <ActionIcon variant="subtle" onClick={getUserGeoLocation} size="sm">
                <IconRefresh size={16} stroke={1.5} />
            </ActionIcon>
        </Tooltip>
    )}
</Group>
);
}
