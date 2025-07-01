import React from 'react';
import { Trail } from '@trailfinder/db-types/database';
import TrailActions from './TrailActions.tsx';

interface TrailHeaderProps {
    trail: Trail;
}

const TrailHeader: React.FC<TrailHeaderProps> = ({ trail }) => {
    return (
        <div className="bg-white rounded-lg border border-gray-200 shadow-sm p-6 mb-4">
            <div className="flex flex-col gap-4">
                <div className="flex flex-col gap-4">
                    <div>
                        <h1 className="text-3xl font-bold text-gray-900">{trail.name}</h1>
                        <p className="text-gray-600 mt-2">{trail.description}</p>
                    </div>

                    <TrailActions trail={trail} />
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                    <div className="bg-gray-50 rounded-lg p-4 border border-gray-100">
                        <div className="text-sm text-gray-600 mb-1">Vegalengd</div>
                        <div className="text-lg font-semibold">{trail.distance.toFixed(1)} m</div>
                    </div>
                    <div className="bg-gray-50 rounded-lg p-4 border border-gray-100">
                        <div className="text-sm text-gray-600 mb-1">Hækkun</div>
                        <div className="text-lg font-semibold">{trail.elevationGainMeters.toFixed(1)} m</div>
                    </div>
                    <div className="bg-gray-50 rounded-lg p-4 border border-gray-100">
                        <div className="text-sm text-gray-600 mb-1">Erfiðleikastig</div>
                        <div className="text-lg font-semibold">{trail.difficultyLevel}</div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default TrailHeader;