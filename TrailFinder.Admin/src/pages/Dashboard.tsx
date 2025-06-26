// src/pages/Dashboard.tsx
import { Grid, Card, Text, Group, Stack, Table, Badge } from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../lib/supabase';
import { IconRoute, IconUsers } from '@tabler/icons-react';
import { MapContainer, TileLayer, GeoJSON } from 'react-leaflet';
import type {Feature, Geometry} from 'geojson';
import { Layer } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import '../styles/map.css';

interface Trail {
  id: string;
  name: string;
  distance_meters: number;
  elevation_gain_meters: number;
  difficulty_level: 'easy' | 'moderate' | 'hard';
  created_at: string;
  route_geom: any;
}

interface TrailStyle {
  color: string;
  weight: number;
  opacity: number;
}

export function Dashboard() {
  
  //
  // Hlaupaleiðir og notendur
  //
  const { data: stats, isLoading: statsLoading } = useQuery({
    queryKey: ['dashboard-stats'],
    queryFn: async () => {
      const [trails, users] = await Promise.all([
        supabase.from('trails').select('count'),
        supabase.from('users').select('count')
      ]);
      
      return {
        trailCount: trails.count || 0,
        userCount: users.count || 0
      };
    }
  });

  //
  // Nýjustu hlaupaleiðir
  //
  const { data: recentTrails, isLoading: recentTrailsLoading } = useQuery({
    queryKey: ['recent-trails'],
    queryFn: async () => {
      const { data } = await supabase
          .from('trails')
          .select('*')
          .order('created_at', { ascending: false })
          .limit(5);
      return data as Trail[];
    }
  });
  
  //
  // Kort með yfirlit af hlaupaleiðum
  //
  const { data: allTrails, isLoading: allTrailsLoading } = useQuery({
    queryKey: ['all-trails-map'],
    queryFn: async () => {
      const { data } = await supabase
          .from('trails')
          .select('id, name, route_geom, difficulty_level');
      return data as Trail[];
    }
  });
  
  if (statsLoading || recentTrailsLoading || allTrailsLoading) {
    return <Text>Allt að koma...</Text>;
  }
  
  const getDifficultyColor = (difficulty_level: string) => {
    switch(difficulty_level) {
      case 'unknown': return 'gray';
      case 'easy': return 'green';
      case 'moderate': return 'yellow';
      case 'hard': return 'orange';
      case 'extreme': return 'red';
      default: return 'gray';
    } 
  };

  const getTrailStyle = (difficulty_level: string): TrailStyle => ({
    color: difficulty_level === 'easy' ? '#2ecc71' :
        difficulty_level === 'moderate' ? '#f1c40f' : '#e74c3c',
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
      <Stack gap="md">
        <Text size="xl" fw={700}>Yfirlit</Text>


        <Grid>
          <Grid.Col span={{ base: 12, md: 6, lg: 3 }}>
            <Card withBorder padding="lg">
              <Group justify="space-between">
                <div>
                  <Text size="xs" c="dimmed">Hlaupaleiðir</Text>
                  <Text fw={700} size="xl">{stats?.trailCount}</Text>
                </div>
                <IconRoute size={48} color="var(--mantine-color-blue-filled)" />
              </Group>
            </Card>
          </Grid.Col>

          <Grid.Col span={{ base: 12, md: 6, lg: 3 }}>
            <Card withBorder padding="lg">
              <Group justify="space-between">
                <div>
                  <Text size="xs" c="dimmed">Notendur</Text>
                  <Text fw={700} size="xl">{stats?.userCount}</Text>
                </div>
                <IconUsers size={48} color="var(--mantine-color-green-filled)" />
              </Group>
            </Card>
          </Grid.Col>
        </Grid>

        
        <Card withBorder>
          <Text fw={600} mb="md">Nýjustu hlaupaleiðir</Text>
          <Table>
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Nafn</Table.Th>
                <Table.Th>Vegalengd</Table.Th>
                <Table.Th>Hækkun</Table.Th>
                <Table.Th>Erfiðleikastig</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {recentTrails?.map((trail) => (
                  <Table.Tr key={trail.id}>
                    <Table.Td>{trail.name}</Table.Td>
                    <Table.Td>{trail.distance_meters} km</Table.Td>
                    <Table.Td>{trail.elevation_gain_meters}m</Table.Td>
                    <Table.Td>
                      <Badge color={getDifficultyColor(trail.difficulty_level)}>
                        {trail.difficulty_level}
                      </Badge>
                    </Table.Td>
                  </Table.Tr>
              ))}
            </Table.Tbody>
          </Table>
        </Card>


        <Card withBorder>
          <Text fw={600} mb="md">Yfirlit yfir hlaupaleiðir</Text>
          <div style={{ height: '400px', width: '100%' }}>
            <MapContainer
                center={[64.9631, -19.0208]}
                zoom={6}
                style={{ height: '100%', width: '100%' }}
            >
              <TileLayer
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />
              {allTrails?.map((trail) => (
                  <GeoJSON
                      key={trail.id}
                      data={trail.route_geom}
                      pathOptions={getTrailStyle(trail.difficulty_level)}
                      onEachFeature={onEachFeature}
                  />
              ))}
            </MapContainer>
          </div>
        </Card>
  
      </Stack>
  );
}
