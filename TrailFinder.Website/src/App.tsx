// src/App.tsx

import { useEffect } from 'react';
import {
    MantineProvider,
    useMantineColorScheme,
    useMantineTheme,
} from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom';
import { Notifications } from '@mantine/notifications';
import { AppShell, Group, Text, Container, NavLink as MantineNavLink } from '@mantine/core';
import { IconRun } from '@tabler/icons-react';

import { MainMenu } from './components/MainMenu';
import { AppRoutes } from './AppRoutes';
import { theme as baseTheme } from './theme';

const queryClient = new QueryClient();

export function App() {
    return (
        <MantineProvider
            theme={baseTheme}
            defaultColorScheme="light"
        >
            <ColorSchemeAutoSetter />
        </MantineProvider>
    );
}

// Sér component sem keyrir auto‑skiptingu + renderar allt appið
function ColorSchemeAutoSetter() {
    const { setColorScheme } = useMantineColorScheme();
    const theme = useMantineTheme();

    useEffect(() => {
        const hour = new Date().getHours();
        setColorScheme(hour >= 7 && hour < 19 ? 'light' : 'light'); // hold on with the dark ;)
    }, [setColorScheme]);

    return (
        <QueryClientProvider client={queryClient}>
            <Notifications
                position="top-right"
                zIndex={9999}
                autoClose={2000}
                containerWidth={300}
                style={{
                    position: 'fixed',
                    top: 56,
                    right: 16,
                    bottom: 'auto',
                    left: 'auto',
                }}
            />
            <BrowserRouter>
                <AppShell header={{ height: 60 }} padding="md">
                    <AppShell.Header p="xs">
                        <Group
                            h="100%"
                            px="md"
                            justify="space-between"
                            wrap="nowrap"
                        >
                            <Group gap="lg" wrap="nowrap">
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
