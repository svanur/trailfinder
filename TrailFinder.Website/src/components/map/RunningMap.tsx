// components/map/RunningMap.tsx
//import { useEffect } from 'react';
import { MapContainer, TileLayer/*, useMap*/ } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';

export function RunningMap() {
    return (
        <MapContainer
            center={[64.9631, -19.0208]} // Miðja Íslands
            zoom={7}
            style={{ height: '100%', width: '100%' }}
        >
            <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            {/* Route layers will go here */}
        </MapContainer>
    );
}