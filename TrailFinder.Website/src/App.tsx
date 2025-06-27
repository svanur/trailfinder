// App.tsx
import { MantineProvider, createTheme } from '@mantine/core';
import { BrowserRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Layout } from './components/Layout';

const queryClient = new QueryClient();

const theme = createTheme({
    primaryColor: 'blue',
});

export function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <MantineProvider theme={theme} defaultColorScheme="light">
                    <Layout />
                </MantineProvider>
            </BrowserRouter>
        </QueryClientProvider>
    );
}