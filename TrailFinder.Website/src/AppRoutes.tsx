// AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import { NotFound } from "./pages/NotFound.tsx";
import { TrailDetails } from "./pages/TrailDetails.tsx";
import { SearchSection } from './components/SearchSection.tsx';
import { TrailsTable } from './components/TrailsTable.tsx';
import { useState } from 'react'; // Import useState for managing searchTerm
import { Container, Title } from '@mantine/core'; // Import Title for the page heading

export function AppRoutes() {
    const [searchTerm, setSearchTerm] = useState(''); // Manage searchTerm state here

    return (
        <Routes>
            <Route path="/" element={
                // Wrap the content with a Fragment or div
                <>
                    <Container size="lg" py="md"> {/* Add a container for general page padding */}
                        <Title order={1} mb="md">Gönguleiðir</Title> {/* Page title */}
                        <SearchSection searchTerm={searchTerm} setSearchTerm={setSearchTerm} />
                        {/* Pass searchTerm to TrailsTable */}
                        <TrailsTable searchTerm={searchTerm} />
                    </Container>
                </>
            } />
            <Route path="/hlaup/:slug" element={<TrailDetails />} />
            <Route path="*" element={<NotFound />} />
        </Routes>
    );
}
