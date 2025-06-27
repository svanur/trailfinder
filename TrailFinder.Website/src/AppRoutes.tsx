// src/AppRoutes.tsx
import { Routes, Route } from 'react-router-dom';
import {Layout} from "./components/Layout.tsx";
import { Home } from './Pages/Home.tsx';
import { RouteDetails } from './Pages/RouteDetails.tsx';
import { NotFound } from './Pages/NotFound.tsx';

export function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route index element={<Home />} />
                <Route path="route/:id" element={<RouteDetails />} />
                <Route path="*" element={<NotFound />} />
            </Route>
        </Routes>
    );
}