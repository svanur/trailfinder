// components/SearchBar.tsx
import { TextInput, Box } from '@mantine/core';
import { IconSearch } from '@tabler/icons-react';

export function SearchBar() {
    return (
        <Box my="md">
            <TextInput
                placeholder="Leita að hlaupaleiðum..."
                leftSection={<IconSearch size={16} />}
                size="md"
                radius="md"
            />
        </Box>
    );
}