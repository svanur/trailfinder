// components/SearchSection.tsx
import {
    TextInput,
    ActionIcon,
    Collapse,
    Stack,
    Group,
    MultiSelect,
    RangeSlider,
    Text
} from '@mantine/core';
import {
    IconSearch,
    IconFilter,
    IconMountain,
    IconRipple,
    IconMountainOff,
    IconWaveSine,
    IconCircle,
    IconArrowBack,
    IconArrowForward,
    IconBabyCarriage,
    IconWalk,
    IconRun
} from '@tabler/icons-react';
import { useState } from 'react';

const difficultyData = [
    { value: 'easy', label: 'Auðvelt', icon: <IconBabyCarriage size={16} /> },
    { value: 'moderate', label: 'Miðlungs', icon: <IconWalk size={16} /> },
    { value: 'hard', label: 'Erfitt', icon: <IconRun size={16} /> },
    { value: 'extreme', label: 'Mjög erfitt', icon: <IconMountain size={16} /> }
];

const routeData = [
    { value: 'circular', label: 'Hringleið', icon: <IconCircle size={16} /> },
    { value: 'out-and-back', label: 'Fram og til baka', icon: <IconArrowBack size={16} /> },
    { value: 'point-to-point', label: 'Punkt í punkt', icon: <IconArrowForward size={16} /> }
];

const terrainData = [
    { value: 'flat', label: 'Flatlendi', icon: <IconRipple size={16} /> },
    { value: 'rolling', label: 'Öldótt', icon: <IconWaveSine size={16} /> },
    { value: 'hilly', label: 'Hólótt', icon: <IconMountainOff size={16} /> },
    { value: 'mountainous', label: 'Fjöllendi', icon: <IconMountain size={16} /> }
];

export function SearchSection() {
    const [showFilters, setShowFilters] = useState(false);

    return (
        <Stack my="md">
            <TextInput
                placeholder="Leita að hlaupaleiðum..."
                leftSection={<IconSearch size={16} />}
                rightSection={
                    <ActionIcon
                        variant="subtle"
                        color="gray"
                        onClick={() => setShowFilters(!showFilters)}
                        aria-label="Ítarleg leit"
                    >
                        <IconFilter
                            size={18}
                            style={{
                                transform: showFilters ? 'rotate(180deg)' : 'none',
                                transition: 'transform 200ms ease'
                            }}
                        />
                    </ActionIcon>
                }
                size="md"
                radius="md"
            />

            <Collapse in={showFilters}>
                <Stack gap="md">
                    <div>
                        <Text size="sm" fw={500} mb={8}>Vegalengd (km)</Text>
                        <RangeSlider
                            min={0}
                            max={50}
                            step={1}
                            minRange={0}
                            marks={[
                                { value: 0, label: '0' },
                                { value: 25, label: '25' },
                                { value: 50, label: '50' }
                            ]}
                        />
                    </div>

                    <div>
                        <Text size="sm" fw={500} mb={8}>Hæðarmunur (m)</Text>
                        <RangeSlider
                            min={0}
                            max={2000}
                            step={100}
                            minRange={0}
                            marks={[
                                { value: 0, label: '0' },
                                { value: 1000, label: '1000' },
                                { value: 2000, label: '2000' }
                            ]}
                        />
                    </div>

                    <Group grow>
                        <MultiSelect
                            placeholder="Erfiðleikastig"
                            data={difficultyData.map(item => ({
                                value: item.value,
                                label: item.label,
                                leftSection: item.icon
                            }))}
                        />
                        <MultiSelect
                            placeholder="Tegund leiðar"
                            data={routeData.map(item => ({
                                value: item.value,
                                label: item.label,
                                leftSection: item.icon
                            }))}
                        />
                        <MultiSelect
                            placeholder="Undirlag"
                            data={terrainData.map(item => ({
                                value: item.value,
                                label: item.label,
                                leftSection: item.icon
                            }))}
                        />
                    </Group>
                </Stack>
            </Collapse>
        </Stack>
    );
}