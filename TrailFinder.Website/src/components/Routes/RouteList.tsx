// components/routes/RouteList.tsx
import { Stack, Text, Grid, Loader, Center } from '@mantine/core';
import { useQuery } from '@tanstack/react-query';
import { RouteCard } from './RouteCard';
import type {Route, FilterOptions} from '../../types/types';

interface RouteListProps {
    filters: FilterOptions;
}

export function RouteList({ filters }: RouteListProps) {
    // Fetch routes with React Query
    const { data: routes, isLoading, error } = useQuery<Route[]>({
        queryKey: ['routes', filters],
        queryFn: async () => {
            // Hér myndum við bæta við tengingu við backend
            const params = new URLSearchParams();
            if (filters.difficulty?.length) {
                params.append('difficulty', filters.difficulty.join(','));
            }
            if (filters.region?.length) {
                params.append('region', filters.region.join(','));
            }
            if (filters.terrainType?.length) {
                params.append('terrainType', filters.terrainType.join(','));
            }
            if (filters.minDistance !== undefined) {
                params.append('minDistance', filters.minDistance.toString());
            }
            if (filters.maxDistance !== undefined) {
                params.append('maxDistance', filters.maxDistance.toString());
            }

            const response = await fetch(`/api/routes?${params}`);
            if (!response.ok) {
                throw new Error('Villa kom upp við að sækja leiðir');
            }
            return response.json();
        },
    });

    if (isLoading) {
        return (
            <Center style={{ height: 200 }}>
                <Loader size="lg" />
            </Center>
        );
    }

    if (error) {
        return (
            <Center style={{ height: 200 }}>
                <Text color="red">Villa kom upp við að sækja leiðir</Text>
            </Center>
        );
    }

    if (!routes?.length) {
        return (
            <Center style={{ height: 200 }}>
                <Text>Engar leiðir fundust með völdum síum</Text>
            </Center>
        );
    }

    return (
        <Stack gap="md">  {/* spacing -> gap */}
            <Grid>
                {routes.map((route) => (
                    <Grid.Col key={route.id} span={{ base: 12, sm: 6, lg: 12 }}>
                        <RouteCard
                            route={route}
                            onFavoriteClick={(routeId) => {
                                console.log('Favorite clicked:', routeId);
                            }}
                        />
                    </Grid.Col>
                ))}
            </Grid>
        </Stack>
    );
}
