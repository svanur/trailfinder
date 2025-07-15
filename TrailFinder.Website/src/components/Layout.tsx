// components/Layout.tsx
import { AppShell, Group, Title } from '@mantine/core';
import { AppRoutes } from '../AppRoutes';

export function Layout() {
    return (
        <AppShell
            header={{ height: 60 }}
            padding="md"
        >
            <AppShell.Header>
                <Group h="100%" px="md">
                    <Title order={1} size="h3">Hlaupaleiðir</Title>
                </Group>
            </AppShell.Header>

            <AppShell.Main>
                <AppRoutes />
            </AppShell.Main>
        </AppShell>
    );
}