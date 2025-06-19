import { Stack, NavLink } from '@mantine/core';
import { IconHome, IconMap, IconUsers } from '@tabler/icons-react';
import { useLocation, useNavigate } from 'react-router-dom';

export function AdminNavigation() {
    const location = useLocation();
    const navigate = useNavigate();

    return (
        <Stack gap="xs">
            <NavLink
                label="Yfirlit"
                leftSection={<IconHome size={16} />}
                active={location.pathname === '/'}
                onClick={() => navigate('/')}
            />
            <NavLink
                label="Hlaupaleiðir"
                leftSection={<IconMap size={16} />}
                active={location.pathname === '/trails'}
                onClick={() => navigate('/trails')}
            />
            <NavLink
                label="Notendur"
                leftSection={<IconUsers size={16} />}
                active={location.pathname === '/users'}
                onClick={() => navigate('/users')}
            />
        </Stack>
    );
}