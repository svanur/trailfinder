// src/pages/Login.tsx (Example modification for password reset link)
import { TextInput, PasswordInput, Button, Paper, Title, Container, Text, Anchor } from '@mantine/core'; // Added Anchor
import { useState } from 'react';
import { supabase } from '../lib/supabase';
import { useNavigate } from 'react-router-dom';

export function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [message, setMessage] = useState<string | null>(null); // New state for success messages
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setMessage(null); // Clear any previous messages

        try {
            const { error } = await supabase.auth.signInWithPassword({
                email,
                password
            });

            if (error) throw error;

            navigate('/');
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Villa kom upp við innskráningu');
        } finally {
            setLoading(false);
        }
    };

    const handleForgotPassword = async () => {
        setLoading(true);
        setError(null);
        setMessage(null);

        if (!email) {
            setError('Vinsamlegast sláðu inn netfang til að endurstilla lykilorð.');
            setLoading(false);
            return;
        }

        try {
            const { error } = await supabase.auth.resetPasswordForEmail(email, {
                redirectTo: `${window.location.origin}/reset-password` // Important: Set your password reset page URL
            });

            if (error) throw error;

            setMessage('Tölvupóstur hefur verið sendur til að endurstilla lykilorð.');
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Villa kom upp við endurstillingu lykilorðs.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container size={420} my={40}>
            <Title ta="center">
                Velkomin/n í TrailFinder
            </Title>

            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <form onSubmit={handleLogin}>
                    <TextInput
                        label="Netfang"
                        placeholder="netfang@dæmi.is"
                        required
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <PasswordInput
                        label="Lykilorð"
                        placeholder="Lykilorðið þitt"
                        required
                        mt="md"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
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
                        Skrá inn
                    </Button>
                    <Text ta="center" mt="md">
                        <Anchor component="button" type="button" onClick={handleForgotPassword} size="sm">
                            Gleymt lykilorð?
                        </Anchor>
                    </Text>
                </form>
            </Paper>
        </Container>
    );
}