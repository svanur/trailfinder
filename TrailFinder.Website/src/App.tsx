// TrailFinder.Website\src\App.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
// Import AppShell, Text, Container, NavLink, Group, useMantineTheme
import { AppShell, Text, Container, NavLink as MantineNavLink, Group, useMantineTheme } from '@mantine/core';
import { IconRun } from "@tabler/icons-react";
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom';

import { AppRoutes } from './AppRoutes.tsx';
import { MainMenu } from './components/MainMenu.tsx';
import { UserLocation } from './components/UserLocation.tsx'; // Import the new UserLocation component

const queryClient = new QueryClient();

export function App() {
    const theme = useMantineTheme();

    // Callback to receive user location from UserLocation component
    const handleLocationDetected = (location: { lat: number; lng: number }) => {
        console.log("User location detected:", location);
        // Here, you would likely store this location in a global state
        // (e.g., using React Context, Zustand, Jotai, or Redux)
        // so that your HomePage or SearchSection can access it to filter/sort trails.
    };

    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <AppShell
                    header={{ height: 60 }}
                    padding="md"
                >
                    <AppShell.Header p="xs">
                        {/* Main header Group with space-between to push location to the right */}
                        <Group h="100%" px="md" justify="space-between" wrap="nowrap"> {/* Changed justify to space-between */}
                            {/* Left side: Logo + Main Menu Items */}
                            <Group gap="lg" wrap="nowrap"> {/* New inner Group for logo and menu */}
                                {/* Logo/Title */}
                                <Text size="xl" fw={700} component="div">
                                    <MantineNavLink
                                        component={RouterNavLink}
                                        to="/"
                                        label="Hlaupaleiðir"
                                        leftSection={<IconRun size={16} stroke={1.5} />}
                                        variant="subtle"
                                        py={0}
                                        pr="xs"
                                        c={theme.colors.gray[8]}
                                    />
                                </Text>
                                {/* Main Menu Items */}
                                <MainMenu />
                            </Group>

                            {/* Right side: User Location Component */}
                            <UserLocation onLocationDetected={handleLocationDetected} />
                        </Group>
                    </AppShell.Header>

                    <AppShell.Main>
                        <Container>
                            <AppRoutes />
                        </Container>
                    </AppShell.Main>
                </AppShell>
            </BrowserRouter>
        </QueryClientProvider>
    );
}
