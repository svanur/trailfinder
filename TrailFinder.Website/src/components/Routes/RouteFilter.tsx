// components/routes/RouteFilters.tsx
import {
    Paper,
    MultiSelect,
    RangeSlider,
    Stack,
    Text,
    Button,
    Group
} from '@mantine/core';
import { useState, useEffect } from 'react';
import type {FilterOptions} from '../../types/types'; // Notum FilterOptions frá types.ts
// Notum FilterOptions frá types.ts

interface RouteFiltersProps {
    filters: FilterOptions;
    onFilterChange: (filters: FilterOptions) => void;
}

export function RouteFilters({ filters: externalFilters, onFilterChange }: RouteFiltersProps) {
    const [localFilters, setLocalFilters] = useState<FilterOptions>({
        difficulty: [],
        minDistance: 0,
        maxDistance: 50,
        region: [],
        terrainType: [],
    });

    useEffect(() => {
        setLocalFilters(externalFilters);
    }, [externalFilters]);

    const difficulties = [
        { value: 'easy', label: 'Auðvelt' },
        { value: 'moderate', label: 'Miðlungs' },
        { value: 'hard', label: 'Erfitt' },
    ];


    const regions = [
        { value: 'hofudborgarsvaedid', label: 'Höfuðborgarsvæðið' },
        { value: 'sudurland', label: 'Suðurland' },
        { value: 'nordurland', label: 'Norðurland' },
        { value: 'austurland', label: 'Austurland' },
        { value: 'vesturland', label: 'Vesturland' },
    ];

    const terrainTypes = [
        { value: 'malbik', label: 'Malbik' },
        { value: 'mottur', label: 'Möl/Mottur' },
        { value: 'fjallastigar', label: 'Fjallastígar' },
        { value: 'blandad', label: 'Blandað' },
    ];

    const handleFilterChange = (key: keyof FilterOptions, value: any) => {
        const newFilters = { ...localFilters, [key]: value };
        setLocalFilters(newFilters);
        onFilterChange(newFilters);
    };

    const handleReset = () => {
        const resetFilters: FilterOptions = {
            difficulty: [],
            minDistance: 0,
            maxDistance: 50,
            region: [],
            terrainType: [],
        };
        setLocalFilters(resetFilters);
        onFilterChange(resetFilters);
    };

    return (
        <Paper p="md">
            <Stack gap="md">
                <Text fw={500} size="lg">Síur</Text>

                <MultiSelect
                    label="Erfiðleikastig"
                    placeholder="Veldu erfiðleikastig"
                    data={difficulties}
                    value={localFilters.difficulty || []}
                    onChange={(value) => handleFilterChange('difficulty', value)}
                    clearable
                    searchable
                />

                <Stack gap="xs">
                    <Text size="sm">Vegalengd (km)</Text>
                    <RangeSlider
                        min={0}
                        max={50}
                        step={1}
                        label={(value) => `${value}km`}
                        value={[
                            localFilters.minDistance ?? 0,
                            localFilters.maxDistance ?? 50
                        ]}
                        onChange={([min, max]) => {
                            handleFilterChange('minDistance', min);
                            handleFilterChange('maxDistance', max);
                        }}
                        marks={[
                            { value: 0, label: '0km' },
                            { value: 25, label: '25km' },
                            { value: 50, label: '50km' },
                        ]}
                    />
                </Stack>

                <MultiSelect
                    label="Landshluti"
                    placeholder="Veldu landshluta"
                    data={regions}
                    value={localFilters.region || []}
                    onChange={(value) => handleFilterChange('region', value)}
                    clearable
                    searchable
                />

                <MultiSelect
                    label="Undirlag"
                    placeholder="Veldu tegund undrlags"
                    data={terrainTypes}
                    value={localFilters.terrainType || []}
                    onChange={(value) => handleFilterChange('terrainType', value)}
                    clearable
                    searchable
                />

                <Group justify="space-between" mt="md">
                    <Button
                        variant="light"
                        color="gray"
                        onClick={handleReset}
                    >
                        Hreinsa síur
                    </Button>
                    <Button>
                        Leita
                    </Button>
                </Group>
            </Stack>
        </Paper>
    );
}
