// App.tsx
import { AppShell, Text, Container } from '@mantine/core';
import { TrailList } from './components/TrailList';
import {SearchSection} from "./components/SearchSection.tsx";

export function App() {
    return (
        <AppShell
            header={{ height: 60 }}
            padding="md"
        >
            <AppShell.Header p="xs">
                <Text size="xl" fw={700}>TrailFinder</Text>
            </AppShell.Header>

            <AppShell.Main>
                <Container>
                    <SearchSection />
                    <TrailList />
                </Container>
            </AppShell.Main>
        </AppShell>
    );
}