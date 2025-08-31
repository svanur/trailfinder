import React from 'react';
import TrailActions from './TrailActions';
import TrailStats from './TrailStats';
import {Trail} from "@trailfinder/db-types/database";

interface TrailHeaderProps {
    trail: Trail;
}

const TrailHeader: React.FC<TrailHeaderProps> = ({ trail }) => {
    return (
        <div className="bg-white rounded-lg border border-gray-200 shadow-sm p-6 mb-4">
            <div className="flex flex-col gap-6">
                <div className="flex flex-col gap-4">
                    <div>
                        <h1 className="text-3xl font-bold text-gray-900">{trail.name}</h1>
                        <p className="text-gray-600 mt-2">{trail.description}</p>
                    </div>
                    <TrailActions trail={trail} />
                </div>
                <TrailStats trail={trail} />
            </div>
        </div>
    );
};

export default TrailHeader;