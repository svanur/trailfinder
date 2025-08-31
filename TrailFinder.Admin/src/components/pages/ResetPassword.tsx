// src/pages/ResetPassword.tsx (Conceptual)
import { useState, useEffect } from 'react';
import { supabase } from '../../lib/supabase';
import { useNavigate } from 'react-router-dom';
import { PasswordInput, Button, Container, Title, Text, Paper } from '@mantine/core';

export function ResetPassword() {
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [message, setMessage] = useState<string | null>(null);
    const navigate = useNavigate();

    const handlePasswordReset = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setMessage(null);

        if (newPassword !== confirmPassword) {
            setError('Lykilorðin stemma ekki.');
            setLoading(false);
            return;
        }

        try {
            // This update only works if the user is currently in a "recovery" state
            // after clicking the reset password link in their email.
            const { error } = await supabase.auth.updateUser({
                password: newPassword
            });

            if (error) throw error;

            setMessage('Lykilorð hefur verið endurstillt! Vinsamlegast skráðu þig inn með nýja lykilorðinu þínu.');
            setTimeout(() => {
                navigate('/login'); // Redirect to login after a short delay
            }, 3000);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Villa kom upp við að endurstilla lykilorð.');
        } finally {
            setLoading(false);
        }
    };

    // You might need to handle the session or recovery token here
    // Supabase client usually handles the session when redirected from the email link.
    // You can check the current session to ensure the user is in a recovery state.
    useEffect(() => {
        const checkUser = async () => {
            const { data: { user } } = await supabase.auth.getUser();
            if (!user) {
                // If no user session, they might not have come from the email link correctly
                // Or the token is expired. You might want to redirect them to the forgot password flow.
            }
        };
        checkUser();
    }, []);

    return (
        <Container size={420} my={40}>
            <Title ta="center">
                Endurstilla lykilorð
            </Title>

            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <form onSubmit={handlePasswordReset}>
                    <PasswordInput
                        label="Nýtt lykilorð"
                        placeholder="Nýja lykilorðið þitt"
                        required
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                    />
                    <PasswordInput
                        label="Staðfesta lykilorð"
                        placeholder="Staðfestu nýja lykilorðið"
                        required
                        mt="md"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                    />
                    {error && (
                        <Text c="red" size="sm" mt="sm">
                            {error}
                        </Text>
                    )}
                    {message && (
                        <Text c="green" size="sm" mt="sm">
                            {message}
                        </Text>
                    )}
                    <Button fullWidth mt="xl" type="submit" loading={loading}>
                        Endurstilla lykilorð
                    </Button>
                </form>
            </Paper>
        </Container>
    );
}