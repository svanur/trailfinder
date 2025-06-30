// src/components/Navbar.tsx
import { Group, Title } from '@mantine/core';
import { ThemeToggle } from './ThemeToggle';

export function Navbar() {
    return (
        <Group h="100%" px="md" justify="space-between">
            <Title order={1} size="h3">TrailFinder</Title>
            <ThemeToggle />
        </Group>
    );
}