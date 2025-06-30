// AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import {RouteDetails} from "./Pages/RouteDetails.tsx";
import { Home } from './Pages/Home.tsx';
import {NotFound} from "./Pages/NotFound.tsx";


export function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            {/* Aðrar routes koma hér fyrir neðan */}
            <Route path="route/:id" element={<RouteDetails />} />
            <Route path="*" element={<NotFound />} />
            <Route path="*" element={<div>Síða fannst ekki</div>} />
        </Routes>
    );
}