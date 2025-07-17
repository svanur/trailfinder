// App.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AppShell, Text, Container, NavLink as MantineNavLink } from '@mantine/core'; // Alias Mantine's NavLink
import { IconRun } from "@tabler/icons-react";
import { BrowserRouter, NavLink as RouterNavLink } from 'react-router-dom'; // Import BrowserRouter and RouterNavLink

import { AppRoutes } from './AppRoutes.tsx'; // Import AppRoutes

const queryClient = new QueryClient();

export function App() {
    return (
        <QueryClientProvider client={queryClient}>
            {/* BrowserRouter must wrap everything that uses routing */}
            <BrowserRouter>
                <AppShell
                    header={{ height: 60 }}
                    padding="md"
                >
                    <AppShell.Header p="xs">
                        <Text size="xl" fw={700}>
                            {/* Use MantineNavLink with component prop for router integration */}
                            <MantineNavLink
                                component={RouterNavLink} // Tell MantineNavLink to render as RouterNavLink
                                to="/" // Use 'to' prop for react-router-dom
                                label="Hlaupaleiðir"
                                leftSection={<IconRun size={16} stroke={1.5} />}
                            />
                        </Text>
                    </AppShell.Header>

                    <AppShell.Main>
                        <Container>
                            {/* Render AppRoutes here to enable routing */}
                            <AppRoutes />
                        </Container>
                    </AppShell.Main>
                </AppShell>
            </BrowserRouter>
        </QueryClientProvider>
    );
}
