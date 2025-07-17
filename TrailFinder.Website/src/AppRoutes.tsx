// AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import {NotFound} from "./pages/NotFound.tsx";
import {TrailDetails} from "./pages/TrailDetails.tsx";

export function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            {/* Aðrar routes koma hér fyrir neðan */}
            <Route path="/hlaup/:slug" element={<TrailDetails />} />
            <Route path="*" element={<NotFound />} />
            <Route path="*" element={<div>Síða fannst ekki</div>} />
        </Routes>
    );
}
