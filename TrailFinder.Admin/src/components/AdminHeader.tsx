// src/components/AdminHeader.tsx
import { Group, Burger, Text, Menu, rem, UnstyledButton } from '@mantine/core';
import { IconLogout, IconSettings, IconChevronDown } from '@tabler/icons-react';
import { useDisclosure } from '@mantine/hooks';
import { useAuth } from '../contexts/AuthContext';
import { useNavigation } from '../contexts/NavigationContext'; // NEW IMPORT
import { useNavigate } from 'react-router-dom';
import { supabase } from '../lib/supabase';

export function AdminHeader() {
    const [opened, { toggle }] = useDisclosure();
    const { user } = useAuth();
    const { selectedPageName } = useNavigation(); // GET THE SELECTED PAGE NAME
    const navigate = useNavigate();

    const handleLogout = async () => {
        const { error } = await supabase.auth.signOut();
        if (error) {
            console.error('Error logging out:', error.message);
        } else {
            navigate('/login');
        }
    };

    return (
        <Group h="100%" px="md" justify="space-between">
            <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
            <Text fw={700}>Hlaupaleiðir {'>'} {selectedPageName}</Text> {/* USE THE VARIABLE HERE */}

            <Group>
                {user && (
                    <Menu shadow="md" width={200} position="bottom-end">
                        <Menu.Target>
                            <UnstyledButton>
                                <Group gap="xs">
                                    <Text size="sm" fw={500}>{user.email}</Text>
                                    <IconChevronDown style={{ width: rem(16), height: rem(16) }} stroke={1.5} />
                                </Group>
                            </UnstyledButton>
                        </Menu.Target>

                        <Menu.Dropdown>
                            <Menu.Item
                                leftSection={<IconSettings style={{ width: rem(14), height: rem(14) }} />}
                                onClick={() => navigate('/settings')}
                            >
                                Stillingar
                            </Menu.Item>
                            <Menu.Divider />
                            <Menu.Item
                                color="red"
                                leftSection={<IconLogout style={{ width: rem(14), height: rem(14) }} />}
                                onClick={handleLogout}
                            >
                                Útskrá
                            </Menu.Item>
                        </Menu.Dropdown>
                    </Menu>
                )}
            </Group>
        </Group>
    );
}
