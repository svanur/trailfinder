// TrailFinder.Website\src\components\TrailMapAndElevation.tsx

import React, {useCallback, useEffect, useMemo, useState} from 'react';
import { useParams } from 'react-router-dom';
import { useTrail } from '../hooks/useTrail';
import LoadingView from './shared/LoadingView';
import ErrorView from './shared/ErrorView';
import NotFoundView from './shared/NotFoundView';
import { useGpxStorage } from '../hooks/useGpxStorage';
import type { GpxPoint } from '../types/GpxPoint.ts';
import TrailVisualization from './TrailVisualization.tsx';

const TrailMapAndElevation: React.FC = () => {
    const { slug } = useParams<{ slug: string }>();

    if (!slug) {
        // Hér getum við t.d. redirectað á forsíðu eða sýnt villuboð
        return <div>Villa: Engin slóð fannst</div>;
    }

    const trailOptions = {
        slug, // Nú vitum við að slug er örugglega strengur
        userLatitude: null,
        userLongitude: null
    };

    const { data: trail, isLoading, error } = useTrail(trailOptions);

    const { getGpxContent } = useGpxStorage();
    const [gpxData, setGpxData] = useState<string>('');
    const [isLoadingGpx, setIsLoadingGpx] = useState(false);
    const [hoveredPoint, setHoveredPoint] = useState<number | null>(null);

    const loadGpxData = useCallback(async () => {
        if (trail?.routeGeom != null) {
            try {
                setIsLoadingGpx(true);
                const trailInfo = await getGpxContent(trail.id);
                
                if (trailInfo && trailInfo.routeGeom) {
                    // The routeGeom is a LineString where coordinates is an array of [longitude, latitude, elevation]
                    const points = trailInfo.routeGeom.map(
                        (coord: number[]) => ({
                            // Note: In GeoJSON, coordinates are in [longitude, latitude, elevation] order
                            lat: coord[1], // Second element is latitude
                            lng: coord[0], // First element is longitude
                            elevation: coord[2] || 0 // The third element is elevation
                        })
                    );
                    setGpxData(JSON.stringify(points));
                }
            } catch (error) {
                console.error('Failed to load GPX data:', error);
            } finally {
                setIsLoadingGpx(false);
            }
        }
    }, [trail?.id]);

    useEffect(() => {
        loadGpxData();
    }, [loadGpxData]);

    const parsedGpxData = useMemo<GpxPoint[]>(() => {
        if (!gpxData) {
            return [];
        }
        try {
            return JSON.parse(gpxData);
        } catch (e) {
            console.error('Failed to parse GPX data:', e);
            return [];
        }
    }, [gpxData]);


    const handleMapHover = useCallback((point: GpxPoint) => {
        if (!parsedGpxData) return;
        const index = parsedGpxData.findIndex(p =>
            p.lat === point.lat && p.lng === point.lng
        );
        setHoveredPoint(index);
    }, [parsedGpxData]);

    const handleProfileHover = useCallback((index: number) => {
        setHoveredPoint(index);
    }, []);

    if (isLoading || isLoadingGpx) {
        return <LoadingView />;
    }

    if (error) {
        return <ErrorView message="Það er nú eitthvað að þessari hlaupaleið:/" />;
    }

    if (!trail) {
        return <NotFoundView message="Þessi leið er eitthvað týnd :(" />;
    }

    return (
        
            <div className="container mx-auto px-4 py-6">
                <div className="space-y-4">
                    

                    {trail.routeGeom != null && parsedGpxData.length > 0 && (
                        <TrailVisualization
                            parsedGpxData={parsedGpxData}
                            hoveredPoint={hoveredPoint}
                            onMapHover={handleMapHover}
                            onProfileHover={handleProfileHover}
                        />
                    )}
                </div>
            </div>
    );
};

export default TrailMapAndElevation;
