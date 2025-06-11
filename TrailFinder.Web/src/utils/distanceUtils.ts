// src/utils/distanceUtils.ts

export enum DistanceUnit {
    Meters = 'm',
    Kilometers = 'km',
    Miles = 'mi'
}

export const convertDistance = (meters: number, toUnit: DistanceUnit, decimals: number = 1): number => {
    switch (toUnit) {
        case DistanceUnit.Kilometers:
            return Number((meters / 1000).toFixed(decimals));
        case DistanceUnit.Miles:
            return Number((meters / 1609.344).toFixed(decimals));
        case DistanceUnit.Meters:
        default:
            return Number(meters.toFixed(decimals));
    }
};

export const formatDistance = (meters: number, toUnit: DistanceUnit, decimals: number = 1): string => {
    const value = convertDistance(meters, toUnit, decimals);
    return `${value} ${toUnit}`;
};