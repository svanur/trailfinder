import { useEffect, useState, useRef } from 'react';
import {
    Chart as ChartJS,
    LineElement,
    PointElement,
    LinearScale,
    Title,
    Tooltip,
    CategoryScale,
    type ChartOptions,
    type Plugin,
    Filler
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import { Box, LoadingOverlay } from '@mantine/core';

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

// Define the SVG for the running figure from Tabler Icons
// viewBox is set to 24 24, stroke-width to 2, and fill to none to match Tabler Icons' style
const runnerSvg = `<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-run" width="32" height="32" viewBox="0 0 24 24" stroke-width="2" stroke="#E6501C" fill="none" stroke-linecap="round" stroke-linejoin="round">
    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
    <path d="M13 4a1 1 0 1 0 2 0a1 1 0 0 0 -2 0"></path>
    <path d="M4 17l.5 -4l4 -2l1 6"></path>
    <path d="M14 21l-1 -4l-1 -1l-1 -1l-1 -2l.5 -1"></path>
    <path d="M16 11l2 1l.5 -2.5"></path>
    <path d="M7 10l2 -1l.5 -1"></path>
    <path d="M7 3l.5 2"></path>
</svg>`;

// Custom plugin to draw the runner icon
const runnerPlugin: Plugin = {
    id: 'runnerPlugin',
    afterDraw: (chart) => {
        const { ctx } = chart;
        const runnerOptions = (chart.options.plugins as any)?.runnerPlugin;
        const highlightedIndex = runnerOptions?.highlightedIndex;

        if (highlightedIndex !== null && highlightedIndex !== undefined) {
            const meta = chart.getDatasetMeta(0);
            const point = meta.data[highlightedIndex];

            if (point) {
                // Draw the SVG image
                const img = new Image();
                img.src = `data:image/svg+xml;base64,${btoa(runnerSvg)}`;
                img.onload = () => {
                    ctx.save();
                    // Center the image on the data point
                    ctx.drawImage(img, point.x - (img.width / 2), point.y - (img.height / 2), img.width, img.height);
                    ctx.restore();
                };
            }
        }
    }
};

// Register the custom plugin
ChartJS.register(runnerPlugin);

interface ElevationProfileProps {
    elevationData: number[];
    onHoverPoint?: (index: number) => void;
    highlightedIndex: number | null;
}

// Correctly define the type for the chart options, including our custom plugin
type ElevationChartOptions = ChartOptions<'line'> & {
    plugins: {
        runnerPlugin?: {
            highlightedIndex?: number | null;
        }
    }
}

export function ElevationProfile({
                                     elevationData,
                                     onHoverPoint,
                                     highlightedIndex
                                 }: ElevationProfileProps) {
    const [isLoading, setIsLoading] = useState(true);
    const chartRef = useRef(null);

    useEffect(() => {
        // Set loading to false once the elevation data is received
        if (elevationData && elevationData.length > 0) {
            setIsLoading(false);
        }
    }, [elevationData]);

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

    const options: ElevationChartOptions = {
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
        <Box pos="relative" h={200}>
            <LoadingOverlay
                visible={isLoading}
                zIndex={1000}
                overlayProps={{ radius: "sm", blur: 2 }}
            />
            {!isLoading && (
                <Line
                    ref={chartRef}
                    data={data}
                    options={options}
                    plugins={[runnerPlugin]}
                />
            )}
        </Box>
    );
}
