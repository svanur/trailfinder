import { Routes, Route } from 'react-router-dom';
import { NotFound } from "./pages/NotFound.tsx";
import { TrailDetails } from "./pages/TrailDetails.tsx";
import { HomePage } from './pages/HomePage.tsx'; // Import the new HomePage
import { MantineProvider } from '@mantine/core'; // Only import what's needed for other routes

import AboutUsPage from "./pages/AboutUs.tsx";

export function AppRoutes() {
    // Remove filters state from here, it's now managed within HomePage
    // const [filters, setFilters] = useState<TrailFilters>(initialTrailFilters);

    return (
        <Routes>
            <Route path="/" element={<HomePage />} /> 
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