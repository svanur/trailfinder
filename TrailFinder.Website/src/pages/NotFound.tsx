import { Button, Container, Group, Text, Title } from '@mantine/core';
import { useNavigate } from 'react-router-dom';
import { IconRun } from '@tabler/icons-react'; // Import the new icon

export function NotFound() {
    const navigate = useNavigate();

    return (
        <Container ta="center" size="sm" style={{ padding: '4rem 0' }}>  
            <IconRun size={180} stroke={1.5} />  
            <Title order={1} mt="md" mb="xs">
                Úpps! Brautin hvarf!
            </Title>
            <Text c="dimmed" size="lg" mb="xl">
                Kannski er best að fara aftur á stíginn?
            </Text>
            <Group justify="center">
                <Button size="md" onClick={() => navigate('/')}>
                    Til baka á upphafsreit
                </Button>
            </Group>
        </Container>
    );
}