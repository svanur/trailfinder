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

    // Wrap getGpxContent in a useCallback to prevent unnecessary re-renders: `getGpxContent``useCallback`
    const loadGpxData = useCallback(async () => {
        if (trail?.has_gpx) {
            try {
                setIsLoadingGpx(true);
                const gpxBlob = await getGpxContent(trail.id);
                if (gpxBlob) {
                    const text = await gpxBlob.text();
                    setGpxData(text);
                }
            } catch (error) {
                console.error('Failed to load GPX data:', error);
            } finally {
                setIsLoadingGpx(false);
            }
        }
    }, [trail, getGpxContent]);


    useEffect(() => {
        loadGpxData();
    }, [loadGpxData]);
    
    const parsedGpxData = useMemo<GpxPoint[] | null>(() => {
        if (!gpxData) return null;

        const parser = new DOMParser();
        const gpxDoc = parser.parseFromString(gpxData, 'text/xml');
        const points = Array.from(gpxDoc.getElementsByTagName('trkpt')).map(point => ({
            lat: parseFloat(point.getAttribute('lat') || '0'),
            lng: parseFloat(point.getAttribute('lon') || '0'),
            elevation: parseFloat(point.getElementsByTagName('ele')[0]?.textContent || '0')
        }));

        return points;
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

                    {trail.has_gpx && gpxData && parsedGpxData && (
                        <TrailVisualization
                            gpxData={gpxData}
                            parsedGpxData={parsedGpxData}
                            hoveredPoint={hoveredPoint}
                            onMapHover={handleMapHover}
                            onProfileHover={handleProfileHover}
                        />
                    )}

                    {trail.has_gpx && <TrailGpxDownload trail={trail} />}
                </div>
            </div>
        </Layout>
    );
};


export default TrailDetails;
