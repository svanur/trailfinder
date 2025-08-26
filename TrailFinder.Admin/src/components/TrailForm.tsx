'use client';

import { useState, useEffect } from 'react';
import {
    TextInput,
    Textarea,
    NumberInput,
    Select,
    Button,
    Group,
    Box,
    Title,
    Alert,
    FileInput,
    Text,
    Loader
} from '@mantine/core';
import { useForm, isNotEmpty } from '@mantine/form';
import { useQuery } from '@tanstack/react-query';
import { IconAlertCircle } from '@tabler/icons-react';
import { trailsApi, type CreateTrailDto } from '../services/trailsApi'; // Import the new API service
import axios from 'axios';

// Redefine the form's values to be consistent and to match the API's CreateTrailDto
type TrailFormValues = CreateTrailDto & { id?: string };

type GpxFileMetadata = {
    id: string;
    original_file_name: string;
    file_size: number;
};

// Define the props for the component
interface TrailFormProps {
    trailId?: string; // Optional ID for editing an existing trail
    onSuccess: () => void;
}

const difficultyLevels = ['unknown', 'easy', 'moderate', 'hard', 'extreme'];
const routeTypes = ['unknown', 'circular', 'outAndBack', 'pointToPoint'];
const terrainTypes = ['unknown', 'flat', 'rolling', 'hilly', 'mountainous'];
const surfaceTypes = ['unknown', 'trail', 'paved', 'mixed'];

export function TrailForm({ trailId, onSuccess }: TrailFormProps) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [gpxFile, setGpxFile] = useState<File | null>(null);

    const { data: gpxMetadata, isLoading: gpxLoading, error: gpxError, refetch: refetchGpx } = useQuery<GpxFileMetadata | null>({
        queryKey: ['gpx-metadata', trailId],
        queryFn: async () => {
            if (!trailId) return null;
            try {
                const metadata = await trailsApi.getGpxMetadata(trailId);
                return metadata;
            } catch (e) {
                if (axios.isAxiosError(e) && e.response?.status === 404) {
                    return null;
                }
                throw e;
            }
        },
        enabled: !!trailId,
    });

    const form = useForm<TrailFormValues>({
        initialValues: {
            name: '',
            slug: '',
            description: '',
            distanceMeters: null,
            elevationGainMeters: null,
            elevationLossMeters: null,
            difficultyLevel: 'unknown',
            routeType: 'unknown',
            terrainType: 'unknown',
            surfaceType: 'unknown',
        },
        validate: {
            name: isNotEmpty('Trail name is required.'),
            slug: isNotEmpty('Slug is required.'),
        },
    });

    useEffect(() => {
        const fetchTrail = async () => {
            if (!trailId) return;

            setLoading(true);
            try {
                const data = await trailsApi.getById(trailId);
                // This conversion to `TrailFormValues` is now safe because the types are more aligned.
                // We also handle potential `null` values from the API by providing a fallback.
                form.setValues({
                    ...data,
                    description: data.description || '',
                    distanceMeters: data.distanceMeters,
                    elevationGainMeters: data.elevationGainMeters,
                    elevationLossMeters: data.elevationLossMeters,
                });
            } catch (e: any) {
                setError(e.message || 'Failed to load trail data.');
            } finally {
                setLoading(false);
            }
        };

        fetchTrail();
    }, [trailId]);

    const handleSubmit = async (values: TrailFormValues) => {
        setLoading(true);
        setError(null);

        // Helper function to convert form values to API-compatible format
        const getFormValuesForApi = (formValues: TrailFormValues) => {
            const apiValues: CreateTrailDto = {
                ...formValues,
                // Convert empty string to null for optional text fields
                description: formValues.description === '' ? null : formValues.description,
            };
            return apiValues;
        };

        let finalTrailId = trailId;

        try {
            if (trailId) {
                // UPDATE operation
                await trailsApi.update(trailId, getFormValuesForApi(values));
            } else {
                // CREATE operation
                const newTrail = await trailsApi.create(getFormValuesForApi(values));
                finalTrailId = newTrail.id;
            }

            // Step 2: Upload the GPX file if one was selected
            if (gpxFile && finalTrailId) {
                await trailsApi.uploadGpxFile(finalTrailId, gpxFile);
                await refetchGpx();
            }

            // Step 3: All operations succeeded
            onSuccess();
            if (!trailId) {
                form.reset();
                setGpxFile(null);
            }
        } catch (e: any) {
            setError(e.response?.data?.Message || e.message || 'An unexpected error occurred.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box maw={800} mx="auto">
            <Title order={2} ta="center" my="md">
                {trailId ? 'Edit Trail' : 'Create New Trail'}
            </Title>

            {loading && <p>Loading...</p>}
            {error && (
                <Alert icon={<IconAlertCircle size="1rem" />} color="red" title="Error">
                    {error}
                </Alert>
            )}

            <form onSubmit={form.onSubmit(handleSubmit)}>
                <TextInput
                    label="Trail Name"
                    placeholder="Enter a name for the trail"
                    withAsterisk
                    {...form.getInputProps('name')}
                />
                <TextInput
                    mt="md"
                    label="Slug"
                    placeholder="e.g., my-great-trail"
                    withAsterisk
                    {...form.getInputProps('slug')}
                />
                <Textarea
                    mt="md"
                    label="Description"
                    placeholder="Provide a detailed description of the trail"
                    autosize
                    minRows={4}
                    {...form.getInputProps('description')}
                />

                <Box mt="xl">
                    <Title order={4} mb="sm">GPX File</Title>
                    {gpxLoading && <Group><Loader size="sm" mr="xs" /><Text c="dimmed">Checking for existing file...</Text></Group>}
                    {gpxError && <Alert icon={<IconAlertCircle size="1rem" />} color="red" title="GPX File Error">Could not load GPX metadata.</Alert>}

                    {gpxMetadata && !gpxFile ? (
                        <Group>
                            <Text>Existing file: **{gpxMetadata.original_file_name}**</Text>
                            <Button variant="light" onClick={() => setGpxFile(null)}>Replace</Button>
                        </Group>
                    ) : (
                        <FileInput
                            label="Upload GPX File"
                            placeholder="Click to select a file"
                            accept=".gpx"
                            value={gpxFile}
                            onChange={setGpxFile}
                            clearable
                            withAsterisk={!trailId}
                        />
                    )}
                </Box>

                <Group mt="xl" grow>
                    <NumberInput
                        label="Distance (meters)"
                        placeholder="e.g., 5000"
                        {...form.getInputProps('distance_meters')}
                    />
                    <NumberInput
                        label="Elevation Gain (meters)"
                        placeholder="e.g., 150"
                        {...form.getInputProps('elevation_gain_meters')}
                    />
                    <NumberInput
                        label="Elevation Loss (meters)"
                        placeholder="e.g., 100"
                        {...form.getInputProps('elevation_loss_meters')}
                    />
                </Group>

                <Group mt="md" grow>
                    <Select
                        label="Difficulty"
                        placeholder="Select a difficulty level"
                        data={difficultyLevels}
                        {...form.getInputProps('difficulty_level')}
                    />
                    <Select
                        label="Route Type"
                        placeholder="Select a route type"
                        data={routeTypes}
                        {...form.getInputProps('route_type')}
                    />
                </Group>

                <Group mt="md" grow>
                    <Select
                        label="Terrain Type"
                        placeholder="Select a terrain type"
                        data={terrainTypes}
                        {...form.getInputProps('terrain_type')}
                    />
                    <Select
                        label="Surface Type"
                        placeholder="Select a surface type"
                        data={surfaceTypes}
                        {...form.getInputProps('surface_type')}
                    />
                </Group>

                <Group p="right" mt="xl">
                    <Button type="submit" loading={loading}>
                        {trailId ? 'Update Trail' : 'Create Trail'}
                    </Button>
                </Group>
            </form>
        </Box>
    );
}