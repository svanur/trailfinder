// src/pages/UserSettingsPage.tsx
import { useState, useEffect } from 'react';
import {
    Container,
    Title,
    Paper,
    Tabs,
    TextInput,
    PasswordInput,
    Button,
    Text,
    Group,
    Loader,
} from '@mantine/core'; // Add any other Mantine components you need
import { useAuth } from '../contexts/AuthContext';
import { supabase } from '../lib/supabase';
import { IconUser, IconLock, IconMail } from '@tabler/icons-react'; // For tab icons (npm install @tabler/icons-react)

export function UserSettingsPage() {
    const { user, loading: authLoading } = useAuth();

    // This line is where setEmail is declared:
    const [email, setEmail] = useState('');

    //const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmNewPassword, setConfirmNewPassword] = useState('');
    const [newEmail, setNewEmail] = useState('');

    const [updateLoading, setUpdateLoading] = useState(false);
    const [updateError, setUpdateError] = useState<string | null>(null);
    const [updateSuccess, setUpdateSuccess] = useState<string | null>(null);

    useEffect(() => {
        if (user) {
            setEmail(email || ''); // Set initial email from auth context
        }
    }, [user]);

    if (authLoading) {
        return <Container size="md" my={40}><Loader /></Container>; // Show loader while auth context loads
    }

    if (!user) {
        return <Container size="md" my={40}><Text>Vinsamlegast skráðu þig inn til að skoða stillingar.</Text></Container>; // Or redirect
    }

    const clearMessages = () => {
        setUpdateError(null);
        setUpdateSuccess(null);
    };

    // ... (handleUpdateProfile, handleChangePassword, handleChangeEmail functions will go here)

    const handleChangePassword = async () => {
        clearMessages();
        if (newPassword !== confirmNewPassword) {
            setUpdateError('Lykilorðin stemma ekki.');
            return;
        }
        if (newPassword.length < 6) { // Basic validation
            setUpdateError('Lykilorð verður að vera að minnsta kosti 6 stafir.');
            return;
        }

        setUpdateLoading(true);
        try {
            // Supabase allows updating password without current password if user is logged in
            const { error } = await supabase.auth.updateUser({
                password: newPassword,
            });

            if (error) {
                throw error;
            }

            setUpdateSuccess('Lykilorði hefur verið breytt!');
            setNewPassword('');
            setConfirmNewPassword('');
            // Optionally, force re-login if desired for security
            // await supabase.auth.signOut();
            // navigate('/login');
        } catch (err) {
            setUpdateError(err instanceof Error ? err.message : 'Villa kom upp við að breyta lykilorði.');
        } finally {
            setUpdateLoading(false);
        }
    };

    const handleChangeEmail = async () => {
        clearMessages();
        if (!newEmail || !newEmail.includes('@')) { // Basic email validation
            setUpdateError('Vinsamlegast sláðu inn gilt netfang.');
            return;
        }
        if (newEmail === user?.email) {
            setUpdateError('Nýtt netfang má ekki vera það sama og núverandi netfang.');
            return;
        }

        setUpdateLoading(true);
        try {
            const { error } = await supabase.auth.updateUser({
                email: newEmail,
            });

            if (error) {
                throw error;
            }

            // Supabase will send a confirmation email to the new address
            setUpdateSuccess('Tölvupóstur hefur verið sendur á nýtt netfang til staðfestingar.');
            setNewEmail('');
            // The user's email in `useAuth` context will only update after they confirm the new email via the link.
            // You might want to display a message prompting them to check their email.
        } catch (err) {
            setUpdateError(err instanceof Error ? err.message : 'Villa kom upp við að breyta netfangi.');
        } finally {
            setUpdateLoading(false);
        }
    };

    return (
        <Container size={600} my={40}>
            <Title ta="center" mb="lg">
                Stillingar notanda
            </Title>

            <Paper withBorder shadow="md" p={30} radius="md">
                <Tabs defaultValue="profile">
                    <Tabs.List>
                        <Tabs.Tab value="profile" leftSection={<IconUser size={16} />}>
                            Almennar upplýsingar
                        </Tabs.Tab>
                        <Tabs.Tab value="password" leftSection={<IconLock size={16} />}>
                            Breyta lykilorði
                        </Tabs.Tab>
                        <Tabs.Tab value="email" leftSection={<IconMail size={16} />}>
                            Breyta netfangi
                        </Tabs.Tab>
                    </Tabs.List>

                    <Tabs.Panel value="profile" pt="xs">
                        {/* Profile Information Section */}
                        <Text fw={700} mt="md">Netfang:</Text>
                        <Text>{user.email}</Text>
                        <Text fw={700} mt="md">Notanda auðkenni:</Text>
                        <Text>{user.id}</Text>
                        {/* You could add more profile fields here if you store them in user_metadata or a separate table */}
                        {/* For example, if you have user.user_metadata.full_name */}
                        {/* <Text fw={700} mt="md">Fullt nafn:</Text>
                        <TextInput
                            placeholder="Fullt nafn"
                            value={user.user_metadata?.full_name || ''}
                            onChange={(e) => { /* handle change and update */ /* }}
                            readOnly={true} // Or make it editable
                        /> */}
                        <Group mt="xl" justify="flex-end">
                            {updateSuccess && <Text c="green" size="sm">{updateSuccess}</Text>}
                            {updateError && <Text c="red" size="sm">{updateError}</Text>}
                            {/* <Button loading={updateLoading} onClick={handleUpdateProfile}>Vista breytingar</Button> */}
                        </Group>
                    </Tabs.Panel>

                    <Tabs.Panel value="password" pt="xs">
                        {/* Change Password Section */}
                        <form onSubmit={(e) => { e.preventDefault(); handleChangePassword(); }}>
                            {/* Note: Supabase's `updateUser` doesn't require the *current* password for a logged-in user.
                                 However, it's a good security practice to ask for it.
                                 If you require current password, you'd need a server-side function or check against the current session.
                                 For a simpler client-side implementation, we'll only ask for new password.
                                 If you want to enforce current password, you'd need a more complex flow,
                                 potentially involving re-authentication or a server-side call.
                            */}
                            {/* <PasswordInput
                                label="Núverandi lykilorð"
                                placeholder="Sláðu inn núverandi lykilorð"
                                required
                                mt="md"
                                value={currentPassword}
                                onChange={(e) => setCurrentPassword(e.target.value)}
                            /> */}
                            <PasswordInput
                                label="Nýtt lykilorð"
                                placeholder="Sláðu inn nýtt lykilorð"
                                required
                                mt="md"
                                value={newPassword}
                                onChange={(e) => setNewPassword(e.target.value)}
                            />
                            <PasswordInput
                                label="Staðfesta nýtt lykilorð"
                                placeholder="Staðfestu nýtt lykilorð"
                                required
                                mt="md"
                                value={confirmNewPassword}
                                onChange={(e) => setConfirmNewPassword(e.target.value)}
                            />
                            {updateError && <Text c="red" size="sm" mt="sm">{updateError}</Text>}
                            {updateSuccess && <Text c="green" size="sm" mt="sm">{updateSuccess}</Text>}
                            <Button fullWidth mt="xl" type="submit" loading={updateLoading}>
                                Breyta lykilorði
                            </Button>
                        </form>
                    </Tabs.Panel>

                    <Tabs.Panel value="email" pt="xs">
                        {/* Change Email Section */}
                        <form onSubmit={(e) => { e.preventDefault(); handleChangeEmail(); }}>
                            <TextInput
                                label="Nýtt netfang"
                                placeholder="Sláðu inn nýtt netfang"
                                required
                                mt="md"
                                type="email"
                                value={newEmail}
                                onChange={(e) => setNewEmail(e.target.value)}
                            />
                            {updateError && <Text c="red" size="sm" mt="sm">{updateError}</Text>}
                            {updateSuccess && <Text c="green" size="sm" mt="sm">{updateSuccess}</Text>}
                            <Button fullWidth mt="xl" type="submit" loading={updateLoading}>
                                Breyta netfangi
                            </Button>
                        </form>
                    </Tabs.Panel>
                </Tabs>
            </Paper>
        </Container>
    );
}