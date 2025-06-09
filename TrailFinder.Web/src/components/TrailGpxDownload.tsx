// src/components/TrailGpxDownload.tsx
import React, { useState } from 'react';
import { Trail } from '@trailfinder/db-types/database';
import downloadGpxFile from "../utils/downloadGpxFile"

interface TrailGpxDownloadProps {
    trail: Trail;
}

const TrailGpxDownload: React.FC<TrailGpxDownloadProps> = ({ trail }) => {
    const [isDownloading, setIsDownloading] = useState(false);

    const handleDownload = async () => {
        setIsDownloading(true);
        try {
            await downloadGpxFile(trail.id);
        } catch (error) {
            console.error('Download failed:', error);
        } finally {
            setIsDownloading(false);
        }
    };

    return (
        <button
            onClick={handleDownload}
            disabled={isDownloading}
            className={`inline-flex items-center px-4 py-2 ${
                isDownloading ? 'bg-blue-400' : 'bg-blue-600 hover:bg-blue-700'
            } text-white rounded-lg transition-colors`}
        >
            {isDownloading ? (
                <span className="animate-spin mr-2">⟳</span>
            ) : (
                <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                </svg>
            )}
            {isDownloading ? 'Hleð niður...' : 'Hlaða niður GPX'}
        </button>
    );
};

export default TrailGpxDownload;
