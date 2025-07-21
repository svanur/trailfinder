// components/SearchSection.tsx
import {
    TextInput,
    ActionIcon,
    Collapse,
    Stack,
    Group,
    RangeSlider,
    Text,
    Combobox,
    Input,
    Pill,
    PillsInput,
    useCombobox
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
    IconSandbox
} from '@tabler/icons-react';
import {type JSX, useState} from 'react';

interface FilterOption {
    value: string;
    label: string;
    icon: JSX.Element;
}

const FilterSelect = ({ data, placeholder }: { data: FilterOption[], placeholder: string }) => {
    const combobox = useCombobox({
        onDropdownClose: () => combobox.resetSelectedOption(),
        onDropdownOpen: () => combobox.updateSelectedOptionIndex('active'),
    });

    const [value, setValue] = useState<string[]>([]);

    const handleValueSelect = (val: string) =>
        setValue((current) =>
            current.includes(val) ? current.filter((v) => v !== val) : [...current, val]
        );

    const handleValueRemove = (val: string) =>
        setValue((current) => current.filter((v) => v !== val));

    const values = value.map((item) => {
        const option = data.find(opt => opt.value === item);
        return (
            <Pill key={item} withRemoveButton onRemove={() => handleValueRemove(item)}>
                <Group gap={5} wrap="nowrap">
                    {option?.icon}
                    <span>{option?.label}</span>
                </Group>
            </Pill>
        );
    });

    const options = data
        .filter((item) => !value.includes(item.value))
        .map((item) => (
            <Combobox.Option value={item.value} key={item.value}>
                <Group gap={5} wrap="nowrap">
                    {item.icon}
                    <span>{item.label}</span>
                </Group>
            </Combobox.Option>
        ));

    return (
        <Combobox store={combobox} onOptionSubmit={handleValueSelect}>
            <Combobox.DropdownTarget>
                <PillsInput
                    pointer
                    onClick={() => combobox.toggleDropdown()}
                >
                    <Pill.Group>
                        {values.length > 0 ? (
                            values
                        ) : (
                            <Input.Placeholder>{placeholder}</Input.Placeholder>
                        )}
                        <Combobox.EventsTarget>
                            <PillsInput.Field
                                type="hidden"
                                onBlur={() => combobox.closeDropdown()}
                                onKeyDown={(event) => {
                                    if (event.key === 'Backspace' && value.length > 0) {
                                        event.preventDefault();
                                        handleValueRemove(value[value.length - 1]);
                                    }
                                }}
                            />
                        </Combobox.EventsTarget>
                    </Pill.Group>
                </PillsInput>
            </Combobox.DropdownTarget>

            <Combobox.Dropdown>
                <Combobox.Options>
                    {options.length === 0 ? <Combobox.Empty>Allt valið</Combobox.Empty> : options}
                </Combobox.Options>
            </Combobox.Dropdown>
        </Combobox>
    );
};


const surfaceData = [
    { value: 'trail', label: 'Utanvega', icon: <IconBabyCarriage style={{ width: 16, height: 16 }} /> },
    { value: 'paved', label: 'Malbik', icon: <IconWalk style={{ width: 16, height: 16 }} /> },
    { value: 'mixed', label: 'Sandur', icon: <IconSandbox style={{ width: 16, height: 16 }} /> }
];
const difficultyData = [
    { value: 'easy', label: 'Auðvelt', icon: <IconBabyCarriage style={{ width: 16, height: 16 }} /> },
    { value: 'moderate', label: 'Í meðallagi', icon: <IconWalk style={{ width: 16, height: 16 }} /> },
    { value: 'hard', label: 'Erfitt', icon: <IconRun style={{ width: 16, height: 16 }} /> },
    { value: 'extreme', label: 'Mjög erfitt', icon: <IconMountain style={{ width: 16, height: 16 }} /> }
];

const routeData = [
    { value: 'circular', label: 'Hringur', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'outAndBack', label: 'Fram og til baka', icon: <IconArrowBack style={{ width: 16, height: 16 }} /> },
    { value: 'pointToPoint', label: 'A til B', icon: <IconArrowForward style={{ width: 16, height: 16 }} /> }
];

const terrainData = [
    { value: 'flat', label: 'Flatlendi', icon: <IconRipple style={{ width: 16, height: 16 }} /> },
    { value: 'rolling', label: 'Rúllandi', icon: <IconWaveSine style={{ width: 16, height: 16 }} /> },
    { value: 'hilly', label: 'Hæðótt', icon: <IconMountainOff style={{ width: 16, height: 16 }} /> },
    { value: 'mountainous', label: 'Fjalllendi', icon: <IconMountain style={{ width: 16, height: 16 }} /> }
];

const regionData = [
    { value: 'hofudborgarsvaedi', label: 'Höfuðborgarsvæði', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'sudurnes', label: 'Suðurnes', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'vesturland', label: 'Vesturland', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'vestfirdir', label: 'Vestfirðir', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'nordurland-vestra', label: 'Norðurland vestra', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'nordurland-eystra', label: 'Norðurland eystra', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'austurland', label: 'Austurland', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
    { value: 'sudurland', label: 'Suðurland', icon: <IconCircle style={{ width: 16, height: 16 }} /> },
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
                    <Group grow align="flex-start">
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
                            <Text size="sm" fw={500} mb={8}>Hækkun (m)</Text>
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
                    </Group>

                    <Group grow>
                        <FilterSelect
                            data={surfaceData}
                            placeholder="Undirlag"
                        />
                        <FilterSelect
                            data={difficultyData}
                            placeholder="Erfiðleikastig"
                        />
                        <FilterSelect
                            data={routeData}
                            placeholder="Tegund leiðar"
                        />
                        <FilterSelect
                            data={terrainData}
                            placeholder="Undirlag"
                        />
                        <FilterSelect
                            data={regionData}
                            placeholder="Staður"
                        />
                    </Group>
                </Stack>
            </Collapse>
        </Stack>
    );
}
