import { PATHS } from '../config/paths';

export const getGpxFilename = (trailId: string) => `${trailId}.gpx`;

export const getGpxPath = (trailId: string) =>
    `${PATHS.GPX_STORAGE}/${getGpxFilename(trailId)}`;
