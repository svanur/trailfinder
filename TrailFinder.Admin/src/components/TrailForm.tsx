'use client';

import { useState, useEffect } from 'react';
import {
    TextInput,
    Textarea,
    NumberInput,
    Select,
    Button,
    Anchor,
    Group,
    Box,
    Title,
    Alert,
    FileInput,
    Text,
    Loader,
    SegmentedControl,
    Center
} from '@mantine/core';
import { IconEye, IconEyeClosed } from '@tabler/icons-react';
import { useForm, isNotEmpty } from '@mantine/form';
import { useQuery } from '@tanstack/react-query';
import { IconAlertCircle } from '@tabler/icons-react';
import { trailsApi, type CreateTrailDto } from '../services/trailsApi';
import axios from 'axios';

type TrailFormValues = CreateTrailDto & { id?: string };

type GpxFileMetadata = {
    id: string;
    originalFileName: string;
    fileSize: number;
};

interface TrailFormProps {
    trailId?: string;
    onSuccess: () => void;
}

const difficultyLevels = ['unknown', 'easy', 'moderate', 'hard', 'extreme'];
const routeTypes = ['unknown', 'circular', 'outAndBack', 'pointToPoint'];
const terrainTypes = ['unknown', 'flat', 'rolling', 'hilly', 'mountainous'];
const surfaceTypes = ['unknown', 'trail', 'paved', 'mixed'];

export function TrailForm({ trailId, onSuccess }: TrailFormProps) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // REMOVED: const [isActiveValue, setIsActiveValue] = useState('react');

    const [gpxFile, setGpxFile] = useState<File | null>(null);

    const { data: gpxMetadata, isLoading: gpxLoading, error: gpxError, refetch: refetchGpx } = useQuery<GpxFileMetadata | null>({
        queryKey: ['gpx-metadata', trailId],
        queryFn: async () => {
            if (!trailId) return null;
            try {
                return await trailsApi.getGpxMetadata(trailId);
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
            isActive: true, // This is your source of truth
        },
        validate: {
            name: isNotEmpty('Trail name is required.'),
            slug: isNotEmpty('Slug is required.'),
        },
    });

    useEffect(() => {
        const fetchTrail = async () => {
            if (!trailId)
                return;

            setLoading(true);
            try {
                const data = await trailsApi.getById(trailId);

                // REMOVED: setIsActiveValue(data.isActive.toString() || 'false');

                form.setValues({
                    ...data,
                    description: data.description || '',
                    distanceMeters: data.distanceMeters,
                    elevationGainMeters: data.elevationGainMeters,
                    elevationLossMeters: data.elevationLossMeters,
                    // The isActive field is now handled directly by the form state.
                    isActive: data.isActive,
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

        const getFormValuesForApi = (formValues: TrailFormValues) => {
            const apiValues: CreateTrailDto = {
                ...formValues,
                description: formValues.description === '' ? null : formValues.description,
            };
            console.log('apiValues', apiValues);
            return apiValues;
        };

        let finalTrailId = trailId;

        try {
            if (trailId) {
                console.log('values', values);
                await trailsApi.update(trailId, getFormValuesForApi(values));
            } else {
                const newTrail = await trailsApi.create(getFormValuesForApi(values));
                finalTrailId = newTrail.id;
            }

            if (gpxFile && finalTrailId) {
                await trailsApi.uploadGpxFile(finalTrailId, gpxFile);
                await refetchGpx();
            }

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
                            <Text>Existing file: **{gpxMetadata.originalFileName}**</Text>
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
                        {...form.getInputProps('distanceMeters')}
                    />
                    <NumberInput
                        label="Elevation Gain (meters)"
                        placeholder="e.g., 150"
                        {...form.getInputProps('elevationGainMeters')}
                    />
                    <NumberInput
                        label="Elevation Loss (meters)"
                        placeholder="e.g., 100"
                        {...form.getInputProps('elevationLossMeters')}
                    />
                </Group>
                <Group mt="md" grow>
                    <Select
                        label="Difficulty"
                        placeholder="Select a difficulty level"
                        data={difficultyLevels}
                        {...form.getInputProps('difficultyLevel')}
                    />
                    <Select
                        label="Route Type"
                        placeholder="Select a route type"
                        data={routeTypes}
                        {...form.getInputProps('routeType')}
                    />
                </Group>
                <Group mt="md" grow>
                    <Select
                        label="Terrain Type"
                        placeholder="Select a terrain type"
                        data={terrainTypes}
                        {...form.getInputProps('terrainType')}
                    />
                    <Select
                        label="Surface Type"
                        placeholder="Select a surface type"
                        data={surfaceTypes}
                        {...form.getInputProps('surfaceType')}
                    />
                </Group>

                <Group mt="md" grow>
                    <SegmentedControl
                        // Use getInputProps directly on the form field
                        {...form.getInputProps('isActive', { type: 'checkbox' })}
                        // Convert the boolean value to a string for the SegmentedControl
                        value={form.values.isActive.toString()}
                        // Convert the string value from the control back to a boolean for the form
                        onChange={(value) => form.setFieldValue('isActive', value === 'true')}
                        data={[
                            {
                                value: 'true',
                                label: (
                                    <Center style={{ gap: 10 }}>
                                        <IconEye size={16} />
                                        <span>Já, birta á vef</span>
                                    </Center>
                                ),
                            },
                            {
                                value: 'false',
                                label: (
                                    <Center style={{ gap: 10 }}>
                                        <IconEyeClosed size={16} />
                                        <span>Nei, ekki birta á vef</span>
                                    </Center>
                                ),
                            },
                        ]}
                    />
                </Group>
                <Group p="right" mt="xl">
                    <Button type="submit" loading={loading}>
                        {trailId ? 'Update Trail' : 'Create Trail'}
                    </Button>
                    <Anchor href="/">
                        Cancel
                    </Anchor>
                </Group>
            </form>
        </Box>
    );
}