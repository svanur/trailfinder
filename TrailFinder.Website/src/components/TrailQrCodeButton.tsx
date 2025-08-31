import React, { forwardRef, useImperativeHandle, useState } from 'react';
import { ActionIcon, Modal, Stack, Text, SegmentedControl } from '@mantine/core';
import { IconQrcode } from '@tabler/icons-react';
import { QRCodeSVG } from 'qrcode.react';
import type { Trail } from '@trailfinder/db-types';

interface TrailQrCodeButtonProps {
    trail: Trail;
}

export interface TrailQrCodeButtonHandle {
    handleQrCode: () => void;
    isOpened: boolean;
}

type QrType = 'share' | 'gpx';

const TrailQrCodeButton = forwardRef<TrailQrCodeButtonHandle, TrailQrCodeButtonProps>(({ trail }, ref) => {
    const [opened, setOpened] = useState(false);
    const [qrType, setQrType] = useState<QrType>('gpx');

    const baseUrl = window.location.origin;

    const getQrUrl = () => {
        if (qrType === 'share') {
            return window.location.href;
        } else {
            return `${baseUrl}/api/v1/trails/${trail.id}/gpx/${encodeURIComponent(trail.name)}.gpx`;
        }
    };

    const handleQrCode = () => {
        setOpened(true);
    };

    useImperativeHandle(ref, () => ({
        handleQrCode,
        isOpened: opened
    }));

    const handleSegmentedControlChange = (value: string) => {
        setQrType(value as QrType);
    };

    return (
        <React.Fragment>
            <ActionIcon
                onClick={handleQrCode}
                size="lg"
                variant="default"
                aria-label="Sýna QR kóða"
                data-qr-code
            >
            <IconQrcode size={24} />
            </ActionIcon>

            <Modal
                opened={opened}
                onClose={() => setOpened(false)}
                title="QR kóði"
                centered
                size="sm"
            >
                <Stack align="center" gap="md">
                    <SegmentedControl
                        value={qrType}
                        onChange={handleSegmentedControlChange}
                        data={[
                            { label: 'GPX niðurhal', value: 'gpx' },
                            { label: 'Deila leið', value: 'share' }
                        ]}
                    />

                    <QRCodeSVG
                        value={getQrUrl()}
                        size={250}
                        level="H"
                        includeMargin
                    />

                    <Text size="sm" c="dimmed" ta="center">
                        {qrType === 'gpx' ? (
                            'Skannaðu kóðann til að hlaða niður GPX skrá beint í símann þinn'
                        ) : (
                            'Skannaðu kóðann til að deila þessari hlaupaleiðinni'
                        )}
                    </Text>

                    <Text size="xs" c="dimmed" ta="center">
                        {qrType === 'gpx' && (
                            'Opnast sjálfkrafa í Garmin Connect eða öðru GPX forriti'
                        )}
                    </Text>
                </Stack>
            </Modal>
        </React.Fragment>
    );
});

TrailQrCodeButton.displayName = 'TrailQrCodeButton';

export default TrailQrCodeButton;
