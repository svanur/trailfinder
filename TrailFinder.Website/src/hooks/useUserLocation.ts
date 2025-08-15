// src/hooks/useUserLocation.ts

import { useState
    //, useEffect 
} from 'react';

interface UserLocation {
    latitude: number | null;
    longitude: number | null;
    timestamp: number | null;
    error: GeolocationPositionError | null;
    isLoading: boolean;
}

export function useUserLocation(): UserLocation {
    const [location/*, setLocation*/] = useState<UserLocation>({
        latitude: null,
        longitude: null,
        timestamp: null,
        error: null,
        isLoading: true,
    });
/*
    useEffect(() => {
        if (!navigator.geolocation) {
            setLocation(prev => ({
                ...prev,
                error: new GeolocationPositionError(),
                isLoading: false,
            }));
            return;
        }

        const handleSuccess = (pos: GeolocationPosition) => {
            setLocation({
                latitude: pos.coords.latitude,
                longitude: pos.coords.longitude,
                timestamp: pos.timestamp,
                error: null,
                isLoading: false,
            });
        };

        const handleError = (err: GeolocationPositionError) => {
            setLocation(prev => ({
                ...prev,
                error: err,
                isLoading: false,
            }));
        };

        // Options for geolocation
        const options = {
            enableHighAccuracy: true, // Request most precise location
            timeout: 10000,           // Time until error if no location is obtained
            maximumAge: 0,            // Don't use a cached position
        };

        // Watch position to update if the user moves, or getCurrentPosition for a one-time fetch
        const watchId = navigator.geolocation.watchPosition(handleSuccess, handleError, options);
        // Or for a single fetch: navigator.geolocation.getCurrentPosition(handleSuccess, handleError, options);

        return () => {
            // Clean up the watchPosition if used
            navigator.geolocation.clearWatch(watchId);
        };
    }, []);
*/
    return location;
}
