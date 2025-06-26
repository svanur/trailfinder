import { Group, Title } from '@mantine/core';

export function AdminHeader() {
    return (
        <Group style={{ height: '100%' }} px={20} justify="space-between">
            <Title order={3}>TrailFinder Admin</Title>
        </Group>
    );
}