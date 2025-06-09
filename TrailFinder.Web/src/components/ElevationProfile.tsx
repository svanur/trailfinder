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
                tension: 0.1,
                pointRadius: (ctx: any) => {
                    // Highlight the hovered point
                    return ctx.dataIndex === highlightedIndex ? 6 : 0;
                },
                pointBackgroundColor: 'rgb(255, 99, 132)'
            }
        ]
    };


    const options = {
        responsive: true,
        onHover: (_: any, elements: any[]) => {
            if (elements.length > 0 && onHoverPoint) {
                onHoverPoint(elements[0].index);
            }
        },
        plugins: {
            legend: {
                display: false
            },
            title: {
                display: true,
                text: 'Elevation Profile'
            }
        },
        scales: {
            y: {
                title: {
                    display: true,
                    text: 'Elevation (m)'
                }
            }
        }
    };

    return <Line data={data} options={options} />;
};

export default ElevationProfile;
