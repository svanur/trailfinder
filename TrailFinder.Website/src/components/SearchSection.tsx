// components/SearchSection.tsx
import { TextInput, ActionIcon, Collapse, Stack } from '@mantine/core';
import { IconSearch, IconFilter } from '@tabler/icons-react';
import { useState } from 'react';
import { FilterSection } from './FilterSection';

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
                <FilterSection />
            </Collapse>
        </Stack>
    );
}