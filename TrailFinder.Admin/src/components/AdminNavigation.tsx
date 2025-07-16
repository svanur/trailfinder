// src/components/AdminNavigation.tsx
import { Stack, NavLink } from '@mantine/core';
import { IconHome, IconMap, IconUsers } from '@tabler/icons-react'; // Added IconSettings if you use it for the nav link
import { useLocation, useNavigate } from 'react-router-dom';
import { useNavigation } from '../contexts/NavigationContext'; // NEW IMPORT

export function AdminNavigation() {
    const location = useLocation();
    const navigate = useNavigate();
    const { setSelectedPageName } = useNavigation(); // Get the setter function from context

    // Helper function to handle navigation and context update
    const handleNavLinkClick = (path: string, name: string) => {
        setSelectedPageName(name); // Update the context
        navigate(path); // Navigate to the new path
    };

    return (
        <Stack gap="xs">
            <NavLink
                label="Yfirlit"
                leftSection={<IconHome size={16} />}
                active={location.pathname === '/'}
                onClick={() => handleNavLinkClick('/', 'Yfirlit')}
            />
            <NavLink
                label="Hlaupaleiðir"
                leftSection={<IconMap size={16} />}
                active={location.pathname === '/trails'}
                onClick={() => handleNavLinkClick('/trails', 'Hlaupaleiðir')}
            />
            <NavLink
                label="Notendur"
                leftSection={<IconUsers size={16} />}
                active={location.pathname === '/users'}
                onClick={() => handleNavLinkClick('/users', 'Notendur')}
            />
           
        </Stack>
    );
}