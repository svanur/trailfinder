import ActionButton from "./ActionButton";
import { IconQrcode } from "@tabler/icons-react";
import type { Trail } from "@trailfinder/db-types";
import React from "react";

interface TrailQrCodeButtonProps {
    trail: Trail;
}

const TrailQrCodeButton: React.FC<TrailQrCodeButtonProps> = ({ trail }) => {
    console.log('TrailQrCodeButton rendering');
    const handleQrCode = () => {
        console.log('QR code clicked', trail.name);
        alert(`QR code functionality for URL: ${window.location.href} would be shown here.`);
    };

    return (
        <ActionButton
            data-qr-code
            icon={<IconQrcode size={20} />}
            label="QR kóði"
            onClick={handleQrCode}
        />
    );
};

export default TrailQrCodeButton;