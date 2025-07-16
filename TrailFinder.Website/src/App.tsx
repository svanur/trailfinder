// App.tsx
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {AppShell, Text, Container, NavLink} from '@mantine/core';
//import { TrailList } from './components/TrailList';
import {SearchSection} from "./components/SearchSection.tsx";
import {TrailsTable} from "./components/TrailsTable.tsx";
import {IconActivity} from "@tabler/icons-react";

const queryClient = new QueryClient();

export function App() {
    return (
        <QueryClientProvider client={queryClient}>
        <AppShell
            header={{ height: 60 }}
            padding="md"
        >
            <AppShell.Header p="xs">
                <Text size="xl" fw={700}>
                    <NavLink
                        href="/"
                        label="Hlaupaleiðir"
                        leftSection={<IconActivity size={16} stroke={1.5} />}
                    />
                </Text>
            </AppShell.Header>

            <AppShell.Main>
                <Container>
                    <SearchSection />
                    <TrailsTable />
                </Container>
            </AppShell.Main>
        </AppShell>
            </QueryClientProvider>
    );
}