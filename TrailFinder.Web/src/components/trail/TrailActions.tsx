
import React from 'react';
import { Trail } from '@trailfinder/db-types/database';
import { IconShare, IconBookmark, IconQrcode } from '@tabler/icons-react';
import TrailGpxDownload from '../TrailGpxDownload';

interface ActionButtonProps {
    icon: React.ReactNode;
    label: string;
    onClick: () => void;
    variant?: 'default' | 'primary';
    disabled?: boolean;
}

const ActionButton: React.FC<ActionButtonProps> = ({
                                                       icon,
                                                       label,
                                                       onClick,
                                                       variant = 'default',
                                                       disabled = false
                                                   }) => {
    const baseStyles = "inline-flex items-center px-3 py-2 text-sm font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2";
    const variantStyles = variant === 'primary'
        ? "text-white bg-blue-600 hover:bg-blue-700 disabled:bg-blue-400"
        : "text-gray-700 bg-white border border-gray-300 hover:bg-gray-50 disabled:bg-gray-100";

    return (
        <button
            onClick={onClick}
            disabled={disabled}
            className={`${baseStyles} ${variantStyles}`}
        >
            {icon}
            <span className="hidden sm:inline ml-2">{label}</span>
        </button>
    );
};

interface TrailActionsProps {
    trail: Trail;
}

const TrailActions: React.FC<TrailActionsProps> = ({ trail }) => {
    const handleSave = () => {
        // TODO: Implement save functionality
        console.log('Save clicked');
    };

    const handleShare = () => {
        // TODO: Implement share functionality
        console.log('Share clicked');
    };

    const handleQrCode = () => {
        // TODO: Implement QR code functionality
        console.log('QR code clicked');
    };

    return (
        <div className="flex flex-wrap items-center gap-2">
            <ActionButton
                icon={<IconBookmark className="w-5 h-5" />}
                label="Vista"
                onClick={handleSave}
            />

            <ActionButton
                icon={<IconShare className="w-5 h-5" />}
                label="Deila"
                onClick={handleShare}
            />
            
            <ActionButton
                icon={<IconQrcode className="w-5 h-5" />}
                label="QR kóði"
                onClick={handleQrCode}
            />

            {trail.routeGeom != null && (
                <div className="contents">
                    <TrailGpxDownload trail={trail} />
                </div>
            )}
        </div>
    );
};

export default TrailActions;