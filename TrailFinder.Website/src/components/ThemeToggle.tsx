// components/ThemeToggle.tsx
import { Button, Menu } from '@mantine/core';
import { useLocalStorage } from '@mantine/hooks';
import { IconSun, IconMoon, IconDeviceDesktop } from '@tabler/icons-react';

type ColorScheme = 'light' | 'dark' | 'auto';

export function ThemeToggle() {
    const [colorScheme, setColorScheme] = useLocalStorage<ColorScheme>({
        key: 'color-scheme',
        defaultValue: 'auto',
    });

    return (
        <Menu>
            <Menu.Target>
                <Button variant="subtle">
                    {colorScheme === 'auto' && <IconDeviceDesktop size={16} />}
                    {colorScheme === 'light' && <IconSun size={16} />}
                    {colorScheme === 'dark' && <IconMoon size={16} />}
                </Button>
            </Menu.Target>

            <Menu.Dropdown>
                <Menu.Item
                    leftSection={<IconSun size={16} />}
                    onClick={() => setColorScheme('light')}
                >
                    Ljóst þema
                </Menu.Item>
                <Menu.Item
                    leftSection={<IconMoon size={16} />}
                    onClick={() => setColorScheme('dark')}
                >
                    Dökkt þema
                </Menu.Item>
                <Menu.Item
                    leftSection={<IconDeviceDesktop size={16} />}
                    onClick={() => setColorScheme('auto')}
                >
                    Sjálfvirkt
                </Menu.Item>
            </Menu.Dropdown>
        </Menu>
    );
}