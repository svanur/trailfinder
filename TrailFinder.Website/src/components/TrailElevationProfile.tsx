import {useEffect, useRef, useState} from 'react';
import {
    CategoryScale,
    Chart as ChartJS,
    type ChartOptions,
    Filler,
    LinearScale,
    LineElement,
    PointElement,
    Title,
    Tooltip
} from 'chart.js';
import {Line} from 'react-chartjs-2';
import {Box, LoadingOverlay} from '@mantine/core';

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


interface ElevationProfileProps {
    elevationData: number[];
    onHoverPoint?: (index: number) => void;
    highlightedIndex: number | null;
}

// Correctly define the type for the chart options, including our custom plugin
type ElevationChartOptions = ChartOptions<'line'> & {
    plugins: {
    }
}

export function TrailElevationProfile(
    {
        elevationData,
        onHoverPoint,
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
                {intersect: false},
                true
            );

            if (points.length > 0) {
                const firstPoint = points[0];
                onHoverPoint(firstPoint.index);
            }
        },
    };

    return (
        <Box pos="relative" w="100%">
            <LoadingOverlay
                visible={isLoading}
                zIndex={1000}
                overlayProps={{radius: "sm", blur: 2}}
            />
            {!isLoading && (
                <Line
                    ref={chartRef}
                    data={data}
                    options={options}
                />
            )}
        </Box>
    );
}
