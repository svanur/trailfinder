// AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import { NotFound } from "./pages/NotFound.tsx";
import { TrailDetails } from "./pages/TrailDetails.tsx";
import { SearchSection } from './components/SearchSection.tsx';
import { TrailsTable } from './components/TrailsTable.tsx';


export function AppRoutes() {
    return (
        <Routes>
            {/* The Home page should contain SearchSection and TrailsTable */}
            <Route path="/" element={
                <>
                    <SearchSection />
                    <TrailsTable />
                </>
            } />
            <Route path="/hlaup/:slug" element={<TrailDetails />} />
            {/* Only one catch-all route for Not Found */}
            <Route path="*" element={<NotFound />} />
        </Routes>
    );
}

