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
    IconRun,
    IconMap,
    IconMapPin,
    IconMapPins,
    IconCompass
} from '@tabler/icons-react';
import { useState } from 'react';

const difficultyData = [
    { value: 'easy', label: 'Auðvelt', icon: <IconBabyCarriage style={{ width: 16, height: 16 }} /> },
    { value: 'moderate', label: 'Miðlungs', icon: <IconWalk style={{ width: 16, height: 16 }} /> },
    { value: 'hard', label: 'Erfitt', icon: <IconRun style={{ width: 16, height: 16 }} /> },
    { value: 'extreme', label: 'Mjög erfitt', icon: <IconMountain style={{ width: 16, height: 16 }} /> }
];

const routeData = [
    { value: 'circular', label: 'Hringleið', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'out-and-back', label: 'Fram og til baka', icon: <IconArrowBack style={{ width: 16, height: 16 }} /> },
    { value: 'point-to-point', label: 'Punkt í punkt', icon: <IconArrowForward style={{ width: 16, height: 16 }} /> }
];

const terrainData = [
    { value: 'flat', label: 'Flatlendi', icon: <IconRipple style={{ width: 16, height: 16 }} /> },
    { value: 'rolling', label: 'Öldótt', icon: <IconWaveSine style={{ width: 16, height: 16 }} /> },
    { value: 'hilly', label: 'Hólótt', icon: <IconMountainOff style={{ width: 16, height: 16 }} /> },
    { value: 'mountainous', label: 'Fjöllendi', icon: <IconMountain style={{ width: 16, height: 16 }} /> }
];

const regionData = [
    { value: 'hofudborgarsvaedi', label: 'Höfuðborgarsvæði', icon: <IconMapPin style={{ width: 16, height: 16 }} /> },
    { value: 'sudurnes', label: 'Suðurnes', icon: <IconMap style={{ width: 16, height: 16 }} /> },
    { value: 'vesturland', label: 'Vesturland', icon: <IconMapPins style={{ width: 16, height: 16 }} /> },
    { value: 'vestfirdir', label: 'Vestfirðir', icon: <IconCompass style={{ width: 16, height: 16 }} /> },
    { value: 'nordurland-vestra', label: 'Norðurland vestra', icon: <IconMap style={{ width: 16, height: 16 }} /> },
    { value: 'nordurland-eystra', label: 'Norðurland eystra', icon: <IconMapPins style={{ width: 16, height: 16 }} /> },
    { value: 'austurland', label: 'Austurland', icon: <IconCompass style={{ width: 16, height: 16 }} /> },
    { value: 'sudurland', label: 'Suðurland', icon: <IconMapPin style={{ width: 16, height: 16 }} /> }
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
                <Stack gap="xl">
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
                            mb={10}
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
                            mb={10}
                        />
                    </div>

                    <Group grow>
                        <MultiSelect
                            placeholder="Erfiðleikastig"
                            data={difficultyData}
                            comboboxProps={{ withinPortal: true }}
                        />
                        <MultiSelect
                            placeholder="Tegund leiðar"
                            data={routeData}
                            comboboxProps={{ withinPortal: true }}
                        />
                        <MultiSelect
                            placeholder="Undirlag"
                            data={terrainData}
                            comboboxProps={{ withinPortal: true }}
                        />
                        <MultiSelect
                            placeholder="Landshluti"
                            data={regionData}
                            comboboxProps={{ withinPortal: true }}
                        />
                    </Group>
                </Stack>
            </Collapse>
        </Stack>
    );
}