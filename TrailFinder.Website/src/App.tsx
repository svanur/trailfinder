// App.tsx
import { MantineProvider } from '@mantine/core';
import { BrowserRouter } from 'react-router-dom';
import { AppRoutes } from './AppRoutes';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

// Búum til nýjan query client instance
const queryClient = new QueryClient();

export function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <MantineProvider
                    theme={{
                        primaryColor: 'blue',
                    }}
                >
                    <AppRoutes />
                </MantineProvider>
            </BrowserRouter>
        </QueryClientProvider>
    );
}