// src/pages/NotFound.tsx
import { Container, Title, Text, Button, Group } from '@mantine/core';
import { useNavigate } from 'react-router-dom';

export function NotFound() {
    const navigate = useNavigate();

    return (
        <Container>
            <Title>404 - Síða fannst ekki</Title>
            <Text>Því miður fannst síðan sem þú ert að leita að ekki.</Text>
            <Group mt="md">
                <Button onClick={() => navigate('/')}>
                    Fara á forsíðu
                </Button>
            </Group>
        </Container>
    );
}