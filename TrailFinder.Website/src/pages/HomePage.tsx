// TrailFinder.Website\src\pages\HomePage.tsx
import { useState } from 'react';
import { Container, Title, Stack, SegmentedControl, Group, Center, Box, Flex, Tooltip } from '@mantine/core';
import { IconTable, IconLayoutGrid } from '@tabler/icons-react';
import { useMediaQuery } from '@mantine/hooks';

import { SearchSection } from '../components/SearchSection';
import { TrailsTable } from '../components/TrailsTable';
import { TrailCards } from '../components/TrailCards.tsx';
import { initialTrailFilters, type TrailFilters } from '../types/filters';

export function HomePage() {
    const [filters, setFilters] = useState<TrailFilters>(initialTrailFilters);

    // Breakpoint for typical mobile devices (<= 768px)
    const isMobile = useMediaQuery('(max-width: 768px)');
    // New breakpoint for very small mobile devices (e.g., <= 480 px or your desired narrowest point)
    const isExtraSmallScreen = useMediaQuery('(max-width: 480px)');

    // State for view mode: 'table' or 'cards'
    // Default to 'cards' on a small screen or mobile, 'table' on larger desktops
    const [viewMode, setViewMode] = useState<'table' | 'cards'>(isExtraSmallScreen || isMobile ? 'cards' : 'table');

    // Determine if the view toggle should be shown
    const showViewToggle = !isExtraSmallScreen;

    return (
        <Container size="xl" py="xl">
            <Stack gap="xl">
                <Title order={1} ta="center" mt="md" mb="sm">
                    Finndu þína leið
                </Title>

                {/* Combined Search and View Toggle Section */}
                {isMobile ? ( // This handles mobile layouts (stacked)
                    <Stack gap="md">
                        <SearchSection filters={filters} setFilters={setFilters} />
                        {showViewToggle && ( // Only show toggle if not on a small screen
                            <Group justify="center">
                                <SegmentedControl
                                    value={viewMode}
                                    onChange={(value) => setViewMode(value as 'table' | 'cards')}
                                    data={[
                                        {
                                            value: 'table',
                                            label: (
                                                <Center style={{ gap: 10 }}>
                                                    <IconTable size={16} />
                                                </Center>
                                            ),
                                        },
                                        {
                                            value: 'cards',
                                            label: (
                                                <Center style={{ gap: 10 }}>
                                                    <IconLayoutGrid size={16} />
                                                </Center>
                                            ),
                                        },
                                    ]}
                                />
                            </Group>
                        )}
                    </Stack>
                ) : ( // This handles desktop layouts (side-by-side)
                    <Flex justify="space-between" align="center" wrap="nowrap" gap="md">
                        <Box style={{ flexGrow: 1 }}>
                            <SearchSection filters={filters} setFilters={setFilters} />
                        </Box>
                        {showViewToggle && ( // Only show toggle if not on a small screen
                            <SegmentedControl
                                value={viewMode}
                                onChange={(value) => setViewMode(value as 'table' | 'cards')}
                                data={[
                                    {
                                        value: 'table',
                                        label: (
                                            <Center style={{ gap: 10 }}>
                                                <Tooltip label="Tafla" withArrow>
                                                    <IconTable size={16} />
                                                </Tooltip>
                                            </Center>
                                        ),
                                    },
                                    {
                                        value: 'cards',
                                        label: (
                                            <Center style={{ gap: 10 }}>
                                                <Tooltip label="Spjöld" withArrow>
                                                    <IconLayoutGrid size={16} />
                                                </Tooltip> 
                                            </Center>
                                        ),
                                    },
                                ]}
                            />
                        )}
                    </Flex>
                )}

                {/* Conditional Rendering based on viewMode. If small screen, always show cards. */}
                {isExtraSmallScreen || viewMode === 'cards' ? (
                    <TrailCards filters={filters} />
                ) : (
                    <TrailsTable filters={filters} />
                )}
            </Stack>
        </Container>
    );
}
