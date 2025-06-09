// src/components/ElevationProfile.tsx
import React from 'react';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
} from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

interface ElevationProfileProps {
    elevationData: number[];
    onHoverPoint?: (index: number) => void;
    highlightedIndex: number | null;
}

const ElevationProfile: React.FC<ElevationProfileProps> = ({
                                                               elevationData,
                                                               onHoverPoint,
                                                               highlightedIndex
                                                           }) => {
    const data = {
        labels: elevationData.map((_, i) => i),
        datasets: [
            {
                label: 'Elevation',
                data: elevationData,
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgba(75, 192, 192, 0.5)',
                tension: 0.1,
                pointRadius: (ctx: any) => {
                    return ctx.dataIndex === highlightedIndex ? 6 : 0;
                },
                pointBackgroundColor: 'rgb(255, 99, 132)',
                fill: true,
            }
        ]
    };

    const options = {
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
            intersect: false,
            mode: 'index' as const,
        },
        plugins: {
            legend: {
                display: false,
            },
            tooltip: {
                enabled: true,
                mode: 'index' as const,
                intersect: false,
            },
        },
        scales: {
            y: {
                title: {
                    display: true,
                    text: 'Elevation (m)',
                },
            },
            x: {
                display: false,
            },
        },
        onHover: (event: any, _: any[], chart: any) => {
            if (!event || !onHoverPoint) return;

            const points = chart.getElementsAtEventForMode(
                event,
                'index',
                { intersect: false },
                true
            );

            if (points.length > 0) {
                const firstPoint = points[0];
                onHoverPoint(firstPoint.index);
            }
        },
    };

    return (
        <div style={{ height: '200px' }}>
            <Line data={data} options={options} />
        </div>
    );
};

export default ElevationProfile;
