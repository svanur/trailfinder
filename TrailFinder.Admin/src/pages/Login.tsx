// src/pages/Login.tsx
import { TextInput, PasswordInput, Button, Paper, Title, Container, Text } from '@mantine/core';
import { useState } from 'react';
import { supabase } from '../lib/supabase';
import { useNavigate } from 'react-router-dom';

export function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

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
                    <Button fullWidth mt="xl" type="submit" loading={loading}>
                        Skrá inn
                    </Button>
                </form>
            </Paper>
        </Container>
    );
}