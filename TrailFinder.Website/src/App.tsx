// TrailFinder.Website\src\App.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AppShell, Text, Container, NavLink as MantineNavLink, Group, useMantineTheme } from '@mantine/core';
import { IconRun } from "@tabler/icons-react";
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom';

import { AppRoutes } from './AppRoutes.tsx';
import { MainMenu } from './components/MainMenu.tsx'; // Import the new component

const queryClient = new QueryClient();

export function App() {
    const theme = useMantineTheme();

    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <AppShell
                    header={{ height: 60 }}
                    padding="md"
                >
                    <AppShell.Header p="xs">
                        <Group h="100%" px="md" justify="flex-start" gap="lg">
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
                            
                            <MainMenu />
                            
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