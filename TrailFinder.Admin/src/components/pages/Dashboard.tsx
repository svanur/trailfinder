// src/pages/Dashboard.tsx
import { Grid, Card, Text, Group, Stack, Loader } from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../../lib/supabase';
import {IconMapPlus, IconRoute, IconUsers} from '@tabler/icons-react';
import { TrailsOverviewMap } from '../dashboard/TrailsOverviewMap';
import {RecentTrailsTable} from "../dashboard/RecentTrailsTable.tsx";
import {Link} from "react-router-dom";

export function Dashboard() {

  //
  // Hlaupaleiðir og notendur stats
  //
  const { data: stats, isLoading: statsLoading, error: statsError } = useQuery({
    queryKey: ['dashboard-stats'],
    queryFn: async () => {
      const [trailsCountResult, userCountResult] = await Promise.all([
        // Query for trail count (still from 'trails' table in public schema)
        supabase.from('trails').select('count', { count: 'exact' }),
        // Call the new RPC function for user count
        supabase.rpc('get_auth_user_count')
      ]);

      if (trailsCountResult.error) throw trailsCountResult.error;
      if (userCountResult.error) throw userCountResult.error; // RPC errors are returned directly

      return {
        trailCount: trailsCountResult.count || 0,
        userCount: userCountResult.data || 0 // RPC returns the count directly in .data
      };
    }
  });

  if (statsLoading) {
    return <Loader />;
  }

  if (statsError) {
    return <Text c="red">Gat ekki hlaðið tölfræði: {statsError.message}</Text>;
  }

  return (
      <Stack gap="md">
        <Text size="xl" fw={700}>Yfirlit</Text>

        {/* Dashboard Statistics */}
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

          <Grid.Col span={{ base: 12, md: 6, lg: 3 }}>
            <Link to="/trails/new" style={{ textDecoration: 'none' }}>
              <Card
                  withBorder
                  padding="lg"
                  // We use the styles prop to apply a hover effect
                  styles={(theme) => ({
                    root: {
                      transition: 'background-color 0.2s ease',
                      '&:hover': {
                        backgroundColor: theme.colors.gray[0],
                        cursor: 'pointer',
                      },
                    },
                  })}
              >
                <Group justify="space-between" wrap="nowrap">
                  <div>
                    <Text fw={700} size="xl">
                      Skrá
                    </Text>
                    <Text fw={500} size="sm" mt="xs" c="dimmed">
                      hlaupaleið
                    </Text>
                  </div>
                  <IconMapPlus size={48} color="var(--mantine-color-green-filled)" />
                </Group>
              </Card>
            </Link>
          </Grid.Col>
          
        </Grid>

        {/* Recent Trails Table Component */}
        <RecentTrailsTable />

        {/* Trails Overview Map Component */}
        <TrailsOverviewMap />

      </Stack>
  );
}