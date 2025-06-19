import { 
    AppShell, 
    Text, 
    MantineProvider 
} from '@mantine/core';
import { AdminNavigation } from './components/AdminNavigation';
import { AdminHeader } from './components/AdminHeader';
import '@mantine/core/styles.css';

function App() {
    return (
        <MantineProvider>
            <AppShell
                padding="md"
                layout="alt"
                navbar={{ width: 250, breakpoint: 'sm' }}
                header={{ height: 60 }}
            >
                <AppShell.Navbar p="xs">
                    <AdminNavigation />
                </AppShell.Navbar>

                <AppShell.Header p="xs">
                    <AdminHeader />
                </AppShell.Header>

                <AppShell.Main>
                    <Text>Velkomin/n í TrailFinder stjórnborðið</Text>
                </AppShell.Main>
            </AppShell>
        </MantineProvider>
    );
}

export default App;