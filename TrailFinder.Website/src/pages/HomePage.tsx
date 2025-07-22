// src/pages/HomePage.tsx
import { useState } from 'react';
import { Container, Title, Text, Stack, SegmentedControl, Group, Center, Box, Flex } from '@mantine/core';
import { IconTable, IconLayoutGrid } from '@tabler/icons-react';
import { useMediaQuery } from '@mantine/hooks';

import { SearchSection } from '../components/SearchSection';
import { TrailsTable } from '../components/TrailsTable';
import { TrailList } from '../components/TrailList';
import { initialTrailFilters, type TrailFilters } from '../types/filters';

export function HomePage() {
    const [filters, setFilters] = useState<TrailFilters>(initialTrailFilters);

    const isMobile = useMediaQuery('(max-width: 768px)');

    const [viewMode, setViewMode] = useState<'table' | 'cards'>(isMobile ? 'cards' : 'table');

    return (
        <Container size="xl" py="xl">
            <Stack gap="xl">
                <Title order={1} ta="center" mt="md" mb="sm">
                    Finndu þína fullkomnu hlaupaleið
                </Title>

                {/* Combined Search and View Toggle Section */}
                {isMobile ? (
                    // On mobile, stack them vertically for better usability
                    <Stack gap="md">
                        <SearchSection filters={filters} setFilters={setFilters} />
                        <Group justify="center"> {/* Center the toggle on mobile */}
                            <SegmentedControl
                                value={viewMode}
                                onChange={(value) => setViewMode(value as 'table' | 'cards')}
                                data={[
                                    {
                                        value: 'table',
                                        label: (
                                            <Center style={{ gap: 10 }}>
                                                <IconTable size={16} />
                                                <Box>Tafla</Box>
                                            </Center>
                                        ),
                                    },
                                    {
                                        value: 'cards',
                                        label: (
                                            <Center style={{ gap: 10 }}>
                                                <IconLayoutGrid size={16} />
                                                <Box>Spjöld</Box>
                                            </Center>
                                        ),
                                    },
                                ]}
                            />
                        </Group>
                    </Stack>
                ) : (
                    // On desktop, place them side-by-side with appropriate spacing
                    <Flex justify="space-between" align="center" wrap="nowrap" gap="md">
                    <Box style={{ flexGrow: 1 }}>
                <SearchSection filters={filters} setFilters={setFilters} />
            </Box>
            <SegmentedControl
                value={viewMode}
                onChange={(value) => setViewMode(value as 'table' | 'cards')}
                data={[
                    {
                        value: 'table',
                        label: (
                            <Center style={{ gap: 10 }}>
                                <IconTable size={16} />
                                <Box>Tafla</Box>
                            </Center>
                        ),
                    },
                    {
                        value: 'cards',
                        label: (
                            <Center style={{ gap: 10 }}>
                                <IconLayoutGrid size={16} />
                                <Box>Spjöld</Box>
                            </Center>
                        ),
                    },
                ]}
            />
        </Flex>
)}


{/* Conditional Rendering based on viewMode */}
{viewMode === 'table' ? (
        <TrailsTable filters={filters} />
    ) : (
        <TrailList filters={filters} />
    )}

    <Text ta="center" c="dimmed" mt="xl">
        Sýni X hlaupaleiðir sem passa við valdar síur.
    </Text>
</Stack>
</Container>
);
}