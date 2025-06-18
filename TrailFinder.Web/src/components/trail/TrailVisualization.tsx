import TrailMap from "../TrailMap.tsx";
import ElevationProfile from "../ElevationProfile.tsx";

interface TrailVisualizationProps {
    parsedGpxData: GpxPoint[];
    hoveredPoint: number | null;
    onMapHover: (point: GpxPoint) => void;
    onProfileHover: (index: number) => void;
}

const TrailVisualization: React.FC<TrailVisualizationProps> = ({
                                                                   parsedGpxData,
                                                                   hoveredPoint,
                                                                   onMapHover,
                                                                   onProfileHover
                                                               }) => (
    <>
        <div className="bg-white rounded-lg shadow-lg p-6 mb-6">
            <TrailMap
                points={parsedGpxData}
                onHoverPoint={onMapHover}
                highlightedPoint={hoveredPoint !== null ? parsedGpxData[hoveredPoint] : null}
            />
        </div>
        <div className="bg-white rounded-lg shadow-lg p-6 mb-6">
            <ElevationProfile
                elevationData={parsedGpxData.map(p => p.elevation)}
                onHoverPoint={onProfileHover}
                highlightedIndex={hoveredPoint}
            />
        </div>
    </>
);

export default TrailVisualization;
