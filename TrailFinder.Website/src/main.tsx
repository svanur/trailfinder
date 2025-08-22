// main.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Notifications } from '@mantine/notifications';

import '@mantine/core/styles.css';
import '@mantine/notifications/styles.css';

// Global styles
import './styles/globals.css';

// Leaflet styles
import 'leaflet/dist/leaflet.css';
import './styles/leaflet-overrides.css';
import { App } from './App';

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <MantineProvider>
                <Notifications
                    position="top-right"
                    zIndex={9999}
                    autoClose={2000}
                    containerWidth={300}
                    style={{
                        position: 'fixed',
                        top: 56,
                        right: 16,
                        bottom: 'auto',
                        left: 'auto',
                    }}
                />
                    <App />
            </MantineProvider>
        </QueryClientProvider>
    </React.StrictMode>
);
