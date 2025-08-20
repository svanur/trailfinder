import React, {useCallback, useEffect, useMemo, useState} from 'react';
import { Grid, Paper, Title, Center } from '@mantine/core';
import {TrailMap} from "./TrailMap.tsx";
import {TrailElevationProfile} from "./TrailElevationProfile.tsx";
import {useParams} from "react-router-dom";
import {useTrail} from "../hooks/useTrail.ts";
import {useGpxStorage} from "../hooks/useGpxStorage.ts";
import type {GpxPoint} from "../types/GpxPoint.ts";
import LoadingView from "./shared/LoadingView.tsx";
import ErrorView from "./shared/ErrorView.tsx";
import NotFoundView from "./shared/NotFoundView.tsx";



/**
 * Renders a responsive container for trail charts using Mantine UI.
 * It displays a map and elevation profile side-by-side on larger screens
 * and stacks them vertically on mobile devices.
 */
const TrailCharts: React.FC = () => {
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
        // Mantine's Grid component handles the responsive layout.
        // On screens with a medium breakpoint (md) and larger, it acts as a row.
        // On screens smaller than md, it automatically stacks the items vertically.
        <Grid gutter="md">
            {/* Trail Map Container */}
            <Grid.Col span={{ base: 12, md: 9 }}>
                <Paper shadow="sm" radius="md" p="md" withBorder>
                    <Title order={3} size="h4" mb="xs">
                        Trail Map
                    </Title>
                    <Center style={{ height: '360px' }}>
                        <TrailMap
                            points={parsedGpxData}
                            onHoverPoint={handleMapHover}
                            highlightedPoint={hoveredPoint !== null ? parsedGpxData[hoveredPoint] : null}
                        />
                    </Center>
                </Paper>
            </Grid.Col>

            {/* Elevation Profile Container */}
            <Grid.Col span={{ base: 12, md: 3 }}>
                <Paper shadow="sm" radius="md" p="md" withBorder>
                    <Title order={3} size="h4" mb="xs">
                        Elevation Profile
                    </Title>
                    <Center style={{ height: '360px' }}>
                        <TrailElevationProfile
                            elevationData={parsedGpxData.map(p => p.elevation)}
                            onHoverPoint={handleProfileHover}
                            highlightedIndex={hoveredPoint}
                        />
                    </Center>
                </Paper>
            </Grid.Col>
        </Grid>
    );
};

export default TrailCharts;
