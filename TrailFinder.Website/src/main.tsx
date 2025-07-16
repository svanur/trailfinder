// main.tsx
import React from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';
import { MantineProvider } from '@mantine/core';
import '@mantine/core/styles.css';
import '@emotion/react';
import './index.css';

const container = document.getElementById('root');
const root = createRoot(container!);
root.render(
    <React.StrictMode>
        <MantineProvider>
            <App />
        </MantineProvider>
    </React.StrictMode>
);