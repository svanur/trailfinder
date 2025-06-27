// pages/Home.tsx
import { useState } from 'react';
import { Container, Grid, Paper } from '@mantine/core';
import { RunningMap } from '../components/map/RunningMap';
import type { FilterOptions } from '../types/types';
import { RouteFilters } from "../components/Routes/RouteFilter.tsx";
import { RouteList } from '../components/Routes/RouteList.tsx';

export function Home() {
    const [filters, setFilters] = useState<FilterOptions>({
        difficulty: [],
        minDistance: 0,
        maxDistance: 50,
        region: [],
        terrainType: [],
    });

    return (
        <Container size="xl" py="xl" style={{ flex: 1 }}>
            <Grid style={{ minHeight: '100%' }}>
                <Grid.Col span={{ base: 12, md: 4 }}>
                    <Paper shadow="sm" p="md" style={{ height: '100%' }}>
                        <RouteFilters
                            filters={filters}
                            onFilterChange={setFilters}
                        />
                        <RouteList filters={filters} />
                    </Paper>
                </Grid.Col>
                <Grid.Col span={{ base: 12, md: 8 }}>
                    <Paper
                        shadow="sm"
                        style={{
                            height: 'calc(100vh - 2rem)',
                            overflow: 'hidden'
                        }}
                    >
                        <RunningMap />
                    </Paper>
                </Grid.Col>
            </Grid>
        </Container>
    );
}