// src/components/TrailNotFound.tsx (or src/pages/TrailNotFound.tsx)
import {Button, Container, Group, Text, Title} from '@mantine/core';
import {useNavigate, useParams} from 'react-router-dom'; // Import useParams
import {IconMapPinOff, IconRun} from '@tabler/icons-react'; // New icons

export function TrailNotFound() {
    const navigate = useNavigate();
    const {slug} = useParams<{ slug: string }>(); // Get the missing slug from the URL

    return (
        <Container ta="center" size="sm" style={{padding: '4rem 0'}}>
            <Group justify="center" align="flex-end" mb="md"> {/* Group icons for alignment */}
                <IconMapPinOff size={120} stroke={1.5}/>
                <IconRun size={80} stroke={1.5}
                         style={{marginLeft: '-20px', marginBottom: '10px'}}/> {/* Slightly offset */}
            </Group>
            <Title order={1} mt="md" mb="xs">
                Úps! Þessi slóð fannst ekki!
            </Title>
            <Text c="dimmed" size="lg" mb="xl">
                <Text>
                    Slóðin sem þú leitaðir að {' '}
                    {slug ? `'${slug}'` : ' '} {/* Display the missing slug if available */}
                    virðist ekki vera á kortinu okkar.
                </Text>
                <Text>
                    Kannski er hún ekki til (ennþá?) eða þú hefur villst aðeins af leið.
                </Text>
            </Text>
            <Group justify="center">
                <Button size="md" onClick={() => navigate('/')}>
                    Skoða aðrar slóðir
                </Button>
            </Group>
        </Container>
    );
}