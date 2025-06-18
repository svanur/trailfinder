import React from 'react';
import {
    Chart as ChartJS,
    LineElement,
    PointElement,
    LinearScale,
    Title,
    Tooltip,
    CategoryScale,
    ChartType,
    ChartOptions,
    Plugin,
    Filler
} from 'chart.js';

import { Line } from 'react-chartjs-2';

// Register the Filler plugin
ChartJS.register(Filler);

// Register the chart.js components
ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip
);

// Declare plugin options type
declare module 'chart.js' {
    interface PluginOptionsByType<TType extends ChartType> {
        runnerPlugin?: {
            highlightedIndex?: number | null;
        };
    }
}

// Define the custom plugin
const runnerPlugin: Plugin = {
    id: 'runnerPlugin',
    afterDraw: (chart) => {
        const { ctx } = chart;
        const runnerOptions = chart.options.plugins?.runnerPlugin;
        const highlightedIndex = runnerOptions?.highlightedIndex;

        if (highlightedIndex !== null && highlightedIndex !== undefined) {
            const meta = chart.getDatasetMeta(0);
            const point = meta.data[highlightedIndex];

            if (point) {
                ctx.save();
                // Use the Material Icons font
                ctx.font = '24px "Material Icons"';
                ctx.fillStyle = '#333';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                // Use the icon's Unicode character
                ctx.fillText('\ue566', point.x, point.y); // This is the Unicode for directions_run
                ctx.restore();
            }
        }
    }
};

// Register the runner plugin
ChartJS.register(runnerPlugin);


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
                label: 'Hæð',
                data: elevationData,
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgba(75, 192, 192, 0.5)',
                tension: 0.1,
                pointRadius: 0,
                pointHoverRadius: 0,
                fill: true,
            }
        ]
    };

    const runnerPlugin = {
        id: 'runnerIcon',
        afterDraw: (chart: ChartJS) => {
            if (highlightedIndex !== null) {
                const ctx = chart.ctx;
                const xAxis = chart.scales.x;
                const yAxis = chart.scales.y;
                const x = xAxis.getPixelForValue(highlightedIndex);
                const y = yAxis.getPixelForValue(elevationData[highlightedIndex]);

                ctx.save();
                ctx.font = '20px "Material Symbols Outlined"';
                ctx.fillStyle = '#2563eb';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                ctx.fillText('directions_run', x, y);
                ctx.restore();
            }
        }
    };

    const options: ChartOptions<'line'> = {
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
            runnerPlugin: {
                highlightedIndex
            }
        },
        scales: {
            y: {
                title: {
                    display: true,
                    text: 'Hæð (m)',
                },
            },
            x: {
                display: false,
            }
        },
        onHover(e, _, chart) {
            if (!onHoverPoint || !e.native) return;

            const points = chart.getElementsAtEventForMode(
                e.native,
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
            <Line
                data={data}
                options={options}
                plugins={[runnerPlugin]}
            />
        </div>
    );
};

export default ElevationProfile;
