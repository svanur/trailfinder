// src/pages/TrailDetails.tsx
import React, {useCallback, useEffect, useMemo, useState} from 'react';
import { useParams } from 'react-router-dom';
import Layout from '../components/layout/Layout';
import { useTrail } from '../hooks/useTrail';
import TrailGpxDownload from '../components/TrailGpxDownload.tsx';
import TrailVisualization from '../components/trail/TrailVisualization.tsx';
import TrailHeader from '../components/trail/TrailHeader.tsx';
import LoadingView from '../components/shared/LoadingView';
import ErrorView from '../components/shared/ErrorView';
import NotFoundView from '../components/shared/NotFoundView';
import { useGpxStorage } from '../hooks/useGpxStorage';

const TrailDetails: React.FC = () => {
    const { slug } = useParams<{ slug: string }>();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');
    const { getGpxContent } = useGpxStorage();
    const [gpxData, setGpxData] = useState<string>('');
    const [isLoadingGpx, setIsLoadingGpx] = useState(false);
    const [hoveredPoint, setHoveredPoint] = useState<number | null>(null);

    const loadGpxData = useCallback(async () => {
        if (trail?.hasGpx) {
            try {
                setIsLoadingGpx(true);
                const trailInfo = await getGpxContent(trail.id);
                if (trailInfo && trailInfo.routeGeom) {
                    const points = trailInfo.routeGeom.map(
                        ([lng, lat, elevation]: number[]) => ({
                            lat,
                            lng,
                            elevation: elevation || 0
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
        return <ErrorView message="Það er nú eitthvað að þessari :/" />;
    }

    if (!trail) {
        return <NotFoundView message="Þessi leið er eitthvað týnd :(" />;
    }


    return (
        <Layout>
            <div className="container mx-auto p-4">
                <div className="bg-white rounded-lg shadow-lg p-6">
                    <TrailHeader trail={trail} />

                    {trail.hasGpx && parsedGpxData.length > 0 && (
                        <TrailVisualization
                            parsedGpxData={parsedGpxData}
                            hoveredPoint={hoveredPoint}
                            onMapHover={handleMapHover}
                            onProfileHover={handleProfileHover}
                        />
                    )}

                    {trail.hasGpx && <TrailGpxDownload trail={trail} />}
                </div>
            </div>
        </Layout>
    );
};


export default TrailDetails;
