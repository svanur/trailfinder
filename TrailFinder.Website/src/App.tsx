// TrailFinder.Website\src\App.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AppShell, Text, Container, NavLink as MantineNavLink, Group, useMantineTheme } from '@mantine/core'; // Added useMantineTheme
import { IconRun, IconInfoCircle, 
//    IconMapPin, IconTrophy 
} from "@tabler/icons-react"; // Import new icons
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom';

import { AppRoutes } from './AppRoutes.tsx';

const queryClient = new QueryClient();

export function App() {
    const theme = useMantineTheme(); // Hook to access Mantine theme

    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <AppShell
                    header={{ height: 60 }}
                    padding="md"
                >
                    <AppShell.Header p="xs">
                        <Group h="100%" px="md" justify="flex-start" gap="lg">
                            {/* Logo/Title - This now serves as the primary Home link */}
                            <Text size="xl" fw={700} component="div">
                                <MantineNavLink
                                    component={RouterNavLink}
                                    to="/"
                                    label="Hlaupaleiðir"
                                    leftSection={<IconRun size={16} stroke={1.5} />}
                                    variant="subtle"
                                    py={0}
                                    pr="xs"
                                    c={theme.colors.gray[8]} // Example: Using a specific gray for the logo text
                                    // Or you could use a hex code: c="#333"
                                />
                            </Text>

                            {/* Navigation Links Group */}
                            <Group gap="md" style={{ display: 'flex', flexDirection: 'row', flexWrap: 'nowrap' }}>
                                
                                {/* Placeholder for future menu items */}
                                {/* Example: "Staðir" */}
                                {/*
                                <MantineNavLink
                                    component={RouterNavLink}
                                    to="/stadir"
                                    label="Staðir"
                                    leftSection={<IconMapPin size={16} stroke={1.5} />}
                                    py={0}
                                    c={theme.colors.gray[7]}
                                />
                                */}
                                {/* Example: "Keppni" */}
                                {/*
                                <MantineNavLink
                                    component={RouterNavLink}
                                    to="/keppni"
                                    label="Keppni"
                                    leftSection={<IconTrophy size={16} stroke={1.5} />}
                                    py={0}
                                    c={theme.colors.gray[7]}
                                />
                                */}

                                <MantineNavLink
                                    component={RouterNavLink}
                                    to="/um"
                                    label="Um"
                                    leftSection={<IconInfoCircle size={16} stroke={1.5} />} // Added icon
                                    py={0}
                                    c={theme.colors.gray[7]} // Dimmed color using Mantine theme
                                    // Or: c="dimmed" for a built-in dimmed effect
                                    // Or: c={theme.colors.dark[4]} for a specific dark shade
                                />
                                
                            </Group>
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
