// App.tsx
import { 
    AppShell, 
    Text, 
    Container 
} from '@mantine/core';
import {SearchBar} from "./components/SearchBar.tsx";
import { FilterSection } from './components/FilterSection.tsx';
import {TrailList} from "./components/TrailList.tsx";


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
                    <SearchBar />
                    <FilterSection />
                    <TrailList />
                </Container>
            </AppShell.Main>
        </AppShell>
    );
}
