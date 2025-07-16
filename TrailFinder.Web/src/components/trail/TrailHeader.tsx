// src/components/trail/TrailHeader.tsx
import {Trail} from "@trailfinder/db-types/database";
import TrailStats from "./TrailStats.tsx";

interface TrailHeaderProps {
    trail: Trail;
}

const TrailHeader: React.FC<TrailHeaderProps> = ({ trail }) => (
    <>
        <h1 className="text-3xl font-bold mb-4">{trail.name}</h1>
        <TrailStats trail={trail} />
        <p className="text-gray-700 mb-6">{trail.description}</p>
    </>
);

export default TrailHeader;
