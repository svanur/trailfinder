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

const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLWlkLWjDqXIiLCJqdGkiOiIxOTYxYTgyMC1kZWQyLTRiZGItOGQ1MC1jODA0NWMwOTFmYzMiLCJuYmYiOjE3NjA0Nzc0ODUsImV4cCI6NDkxNjE1MTA4NSwiaWF0IjoxNzYwNDc3NDg1LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjQxNzMiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQxNzMifQ.ptRezy2NHYKA0C5oBerEBzYEdp4ltUxpd4JgjGifJGw';
localStorage.setItem('bearerToken', token);

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
