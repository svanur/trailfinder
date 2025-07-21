// src/components/SearchSection.tsx
import { TextInput, Container } from '@mantine/core';
import { IconSearch } from '@tabler/icons-react';
import React from 'react'; // Import React for React.Dispatch

interface SearchSectionProps {
    searchTerm: string;
    setSearchTerm: React.Dispatch<React.SetStateAction<string>>;
}

export function SearchSection({ searchTerm, setSearchTerm }: SearchSectionProps) {
    return (
        <Container size="lg" py="md">
            <TextInput
                placeholder="Leita að hlaupaleiðium..."
                value={searchTerm}
                onChange={(event) => setSearchTerm(event.currentTarget.value)}
                leftSection={<IconSearch size={16} />}
                size="md"
                radius="md"
            />
        </Container>
    );
}
