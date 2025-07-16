// src/components/dashboard/TrailsOverviewMap.tsx
import { Card, Text } from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../../lib/supabase';
import { MapContainer, TileLayer, GeoJSON } from 'react-leaflet';
import type { Feature, Geometry } from 'geojson';
import { Layer } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import '../../styles/map.css';

interface Trail {
    id: string;
    name: string;
    difficulty_level: 'unknown' | 'easy' | 'moderate' | 'hard' | 'extreme';
    route_geom: any; // GeoJSON geometry
}

interface TrailStyle {
    color: string;
    weight: number;
    opacity: number;
}

export function TrailsOverviewMap() {
    const { data: allTrails, isLoading: allTrailsLoading, error } = useQuery<Trail[]>({ // Added error
        queryKey: ['all-trails-map'],
        queryFn: async () => {
            const { data, error } = await supabase
                .from('trails')
                .select('id, name, route_geom, difficulty_level');

            if (error) throw error;
            return data as Trail[];
        }
    });

    if (allTrailsLoading) {
        return <Text>Hleð inn korti yfir hlaupaleiðir...</Text>;
    }

    if (error) {
        return <Text c="red">Gat ekki hlaðið korti yfir hlaupaleiðir: {error.message}</Text>;
    }

    const getTrailStyle = (difficulty: Trail['difficulty_level']): TrailStyle => ({
        color: difficulty === 'easy' ? '#2ecc71' :
            difficulty === 'moderate' ? '#f1c40f' : '#e74c3c',
        weight: 3,
        opacity: 0.7
    });

    const onEachFeature = (_feature: Feature<Geometry>, layer: Layer) => {
        if (_feature.properties) {
            layer.bindPopup(`
                <strong>${_feature.properties.name}</strong><br>
                Erfiðleikastig: ${
                _feature.properties.difficulty_level === 'easy' ? 'Létt' :
                    _feature.properties.difficulty_level === 'moderate' ? 'Miðlungs' : 'Erfið'
            }
            `);
        }
    };

    return (
        <Card withBorder>
            <Text fw={600} mb="md">Yfirlit yfir hlaupaleiðir</Text>
            <div style={{ height: '400px', width: '100%' }}>
                <MapContainer
                    center={[64.9631, -19.0208]} // Centered on Iceland
                    zoom={6}
                    style={{ height: '100%', width: '100%' }}
                >
                    <TileLayer
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    />
                    {allTrails && allTrails.length > 0 ? (
                        allTrails.map((trail) => (
                            <GeoJSON
                                key={trail.id}
                                data={trail.route_geom}
                                pathOptions={getTrailStyle(trail.difficulty_level)}
                                onEachFeature={onEachFeature}
                            />
                        ))
                    ) : (
                        <Text c="dimmed" style={{ position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', zIndex: 1000, backgroundColor: 'white', padding: '10px', borderRadius: '4px' }}>
                            Engar hlaupaleiðir til að birta á korti.
                        </Text>
                    )}
                </MapContainer>
            </div>
        </Card>
    );
}