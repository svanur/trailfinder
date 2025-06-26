// src/pages/Dashboard.tsx
import { 
  Grid, 
  Card, 
  Text, 
  Group, 
   
  Stack, 
  Table, 
  Badge 
} from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../lib/supabase';
import {
  IconUsers,
  IconRoute 
} from '@tabler/icons-react';

interface Trail {
  id: string;
  name: string;
  distance_meters: number;
  elevation_gain_meters: number;
  difficulty: 'easy' | 'moderate' | 'hard';
  created_at: string;
}

export function Dashboard() {
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

  const { data: recentTrails, isLoading: trailsLoading } = useQuery({
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

  if (statsLoading || trailsLoading) {
    return <Text>Hleður gögnum...</Text>;
  }

  const getDifficultyColor = (difficulty: string) => {
    switch(difficulty) {
      case 'easy': return 'green';
      case 'moderate': return 'yellow';
      case 'hard': return 'red';
      default: return 'gray';
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
                    <Table.Td>{trail.distance_meters.toFixed(1)} km</Table.Td>
                    <Table.Td>{trail.elevation_gain_meters}m</Table.Td>
                    <Table.Td>
                      <Badge color={getDifficultyColor(trail.difficulty)}>
                        {trail.difficulty === 'easy' ? 'Létt' :
                            trail.difficulty === 'moderate' ? 'Miðlungs' : 'Erfið'}
                      </Badge>
                    </Table.Td>
                  </Table.Tr>
              ))}
            </Table.Tbody>
          </Table>
        </Card>
      </Stack>
  );
}