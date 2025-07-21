// App.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AppShell, Text, Container, NavLink as MantineNavLink } from '@mantine/core';
import { IconRun } from "@tabler/icons-react";
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom';

import { AppRoutes } from './AppRoutes.tsx';

const queryClient = new QueryClient();

export function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <AppShell
                    header={{ height: 60 }}
                    padding="md"
                >
                    <AppShell.Header p="xs">
                        {/* CHANGE HERE: Use component="div" or component="span" for Text */}
                        {/* Or, if you want it to be a semantic heading, use component="h1" (though adjust size/styles accordingly) */}
                        <Text size="xl" fw={700} component="div">
                            <MantineNavLink
                                component={RouterNavLink}
                                to="/"
                                label="Hlaupaleiðir"
                                leftSection={<IconRun size={16} stroke={1.5} />}
                            />
                        </Text>
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