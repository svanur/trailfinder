import { AppShell, MantineProvider } from '@mantine/core';
import { AdminNavigation } from './components/AdminNavigation';
import { AdminHeader } from './components/AdminHeader';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Dashboard } from './pages/Dashboard';
import { Trails } from './pages/Trails';
import { Users } from './pages/Users';
import '@mantine/core/styles.css';

function App() {
    return (
        <BrowserRouter>
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
                        <Routes>
                            <Route path="/" element={<Dashboard />} />
                            <Route path="/trails" element={<Trails />} />
                            <Route path="/users" element={<Users />} />
                        </Routes>
                    </AppShell.Main>
                </AppShell>
            </MantineProvider>
        </BrowserRouter>
    );
}

export default App;