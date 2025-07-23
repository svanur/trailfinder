// TrailFinder.Website\src\components\MainMenu.tsx
import { Group, NavLink as MantineNavLink, useMantineTheme } from '@mantine/core';
import { NavLink as RouterNavLink } from 'react-router-dom';
import { IconInfoCircle, 
//    IconMapPin, IconTrophy 
} from '@tabler/icons-react'; // Import icons needed for menu items

export function MainMenu() {
    const theme = useMantineTheme(); // Use theme for consistent colors

    return (
        <Group gap="md" style={{ display: 'flex', flexDirection: 'row', flexWrap: 'nowrap' }}>
            <MantineNavLink
                component={RouterNavLink}
                to="/um"
                label="Um"
                leftSection={<IconInfoCircle size={16} stroke={1.5} />}
                py={0}
                c={theme.colors.gray[7]} // Consistent dimmed color
            />
            {/* Future menu items can be added here */}
            {/*
            <MantineNavLink
                component={RouterNavLink}
                to="/stadir"
                label="Staðir"
                leftSection={<IconMapPin size={16} stroke={1.5} />}
                py={0}
                c={theme.colors.gray[7]}
            />
            <MantineNavLink
                component={RouterNavLink}
                to="/keppni"
                label="Keppni"
                leftSection={<IconTrophy size={16} stroke={1.5} />}
                py={0}
                c={theme.colors.gray[7]}
            />
            */}
        </Group>
    );
}
