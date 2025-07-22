// AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import { NotFound } from "./pages/NotFound.tsx";
import { TrailDetails } from "./pages/TrailDetails.tsx";
import { SearchSection } from './components/SearchSection.tsx';
import { TrailsTable } from './components/TrailsTable.tsx';
import { useState } from 'react';
import {Container, MantineProvider, Title} from '@mantine/core';
import {type TrailFilters, initialTrailFilters } from './types/filters';
import AboutUsPage from "./pages/AboutUs.tsx"; // Import filter types

export function AppRoutes() {
    // Manage the full filter state object here
    const [filters, setFilters] = useState<TrailFilters>(initialTrailFilters);

    return (
        <Routes>
            <Route path="/" element={
                <>
                    <Container size="lg" py="md">
                        <Title order={1} mb="md">Gönguleiðir</Title>
                        {/* Pass the entire filters object and its setter to SearchSection */}
                        <SearchSection filters={filters} setFilters={setFilters} />
                        {/* Pass the entire filters object to TrailsTable */}
                        <TrailsTable filters={filters} />
                    </Container>
                </>
            } />
            <Route path="/hlaup/:slug" element={<TrailDetails />} />
            <Route path="*" element={<NotFound />} />
            <Route path="/um" element={
                <MantineProvider>
                    <AboutUsPage />
                </MantineProvider>
            } />
        </Routes>
    );
}
