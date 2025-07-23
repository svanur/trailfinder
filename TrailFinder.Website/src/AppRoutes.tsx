// \TrailFinder.Website\src\AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import { NotFound } from "./pages/NotFound.tsx";
import { TrailDetails } from "./pages/TrailDetails.tsx";
import { HomePage } from './pages/HomePage.tsx';
//import { MantineProvider } from '@mantine/core'; // Only import what's needed for other routes

import AboutUsPage from "./pages/AboutUs.tsx";

export function AppRoutes() {

    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="*" element={<NotFound />} />
            
            <Route path="/hlaup/:slug" element={<TrailDetails />} />
            <Route path="/um" element={<AboutUsPage />} />

        </Routes>
    );
}