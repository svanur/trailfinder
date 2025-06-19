import { Stack, NavLink } from '@mantine/core';
import { IconHome, IconMap, IconUsers } from '@tabler/icons-react';

export function AdminNavigation() {
    return (
        <Stack gap="xs">
            <NavLink
                label="Yfirlit"
                leftSection={<IconHome size={16} />}
            />
            <NavLink
                label="Gönguleiðir"
                leftSection={<IconMap size={16} />}
            />
            <NavLink
                label="Notendur"
                leftSection={<IconUsers size={16} />}
            />
        </Stack>
    );
}