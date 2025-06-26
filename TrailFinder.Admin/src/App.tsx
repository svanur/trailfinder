// src/App.tsx
import { BrowserRouter, Route, Routes, Navigate, useLocation } from 'react-router-dom';
import { AppShell, MantineProvider } from '@mantine/core';
import { AdminNavigation } from './components/AdminNavigation';
import { AdminHeader } from './components/AdminHeader';
import { Dashboard } from './pages/Dashboard';
import { Trails } from './pages/Trails';
import { Users } from './pages/Users';
import { Login } from './pages/Login';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import '@mantine/core/styles.css';

function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const { user, loading } = useAuth();
    const location = useLocation();

    if (loading) {
        return <div>Allt að koma...</div>;
    }

    if (!user) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    return children;
}

const queryClient = new QueryClient();

function AppLayout() {
    return (
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
    );
}

function App() {
    return (
        <QueryClientProvider client={queryClient}>
        <AuthProvider>
            <BrowserRouter>
                <MantineProvider>
                    <Routes>
                        <Route path="/login" element={<Login />} />
                        <Route
                            path="/*"
                            element={
                                <ProtectedRoute>
                                    <AppLayout />
                                </ProtectedRoute>
                            }
                        />
                    </Routes>
                </MantineProvider>
            </BrowserRouter>
        </AuthProvider>
        </QueryClientProvider>
    );
}

export default App;