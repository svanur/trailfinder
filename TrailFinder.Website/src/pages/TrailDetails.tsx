// src/pages/TrailDetails.tsx
import { useParams, useNavigate } from 'react-router-dom';
import { useTrail } from '../hooks/useTrail';
import { Container, Title, Text, Group, Stack, Badge, Card, Button } from '@mantine/core'; // Removed Loader import
import { IconRuler, IconMountain, IconRoute, IconExternalLink } from '@tabler/icons-react';

// Import your new custom loader component
import { TrailLoader } from '../components/TrailLoader'; // Adjust path if needed
import { useEffect } from 'react';
import { TrailNotFound } from "./TrailNotFound.tsx";
import {TrailNotFoundError} from "../types/api.ts";


export function TrailDetails() {
    const { slug } = useParams<{ slug: string }>();
    const navigate = useNavigate();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');

    const isTrailSpecificError = error instanceof TrailNotFoundError;
    //const suggestions = isTrailSpecificError ? (error as TrailNotFoundError).suggestions : [];

    // Log errors for debugging, but don't show to user directly unless it's a generic error
    useEffect(() => {
        if (error && !isTrailSpecificError) {
            console.error("General error loading trail:", error);
        }
    }, [error, isTrailSpecificError]);


    if (isLoading) {
        return <TrailLoader />; // Use your custom loader here!
    }

    if (isTrailSpecificError) {
        return (
            <TrailNotFound />
            /*
            <TrailNotFound>
                {suggestions.length > 0 && (
                    <>
                        <Text mt="xl" size="lg" fw={700}>
                            Kannski varstu að leita að einni af þessum slóðum:
                        </Text>
                        <Group spacing="xs" mt="sm" justify="center">
                            {suggestions.map((sugTrail) => (
                                <Button
                                    key={sugTrail.id}
                                    variant="outline"
                                    onClick={() => navigate(`/hlaup/${sugTrail.slug}`)}
                                >
                                    {sugTrail.name}
                                </Button>
                            ))}
                        </Group>
                    </>
                )}
            </TrailNotFound>
            */
        );
    }

    if (error) { // This now only catches non-TrailNotFoundError errors
        return <TrailNotFound />;
        /*
        return (
            <Container ta="center" style={{ padding: '4rem 0' }}>
                <Title order={2}>Úpps!</Title>
                <Text mt="md">Eitthvað fórum við út af stígnum hérna.</Text>
                
                <Group justify="center">
                    <Button size="md" onClick={() => navigate('/')}>
                        Til baka á upphafsreit
                    </Button>
                </Group>
            </Container>
        );
        */
    }

    if (!trail) {
        // This should ideally only happen if 'error' is null but 'data' is also null
        // which might indicate a problem that isn't a 404 or a clear error.
        return (
            <Container ta="center" style={{ padding: '4rem 0' }}>
                <Title order={2}>Engin gögn fundust!</Title>
                <Text mt="md">Einhver villa kom upp og engin gönguleiðargögn fundust.</Text>
                <Button mt="lg" onClick={() => navigate('/')}>Aftur á forsíðu</Button>
            </Container>
        );
    }

    return (
        <Container size="lg">
            <Stack gap="md">
                <Title order={1}>{trail.name}</Title>

                <Card withBorder>
                    <Group gap="xl">
                        <Group>
                            <IconRuler size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Vegalengd</Text>
                                <Text>{trail.distance.toFixed(1)} km</Text>
                            </div>
                        </Group>

                        <Group>
                            <IconMountain size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Hækkun</Text>
                                <Text>{trail.elevationGain} m</Text>
                            </div>
                        </Group>

                        <Group>
                            <IconRoute size={20} />
                            <div>
                                <Text size="sm" c="dimmed">Tegund leiðar</Text>
                                <Text>{trail.routeType}</Text>
                            </div>
                        </Group>
                    </Group>
                </Card>

                <Card withBorder>
                    <Text size="lg" fw={500} mb="md">Um hlaupaleiðina</Text>
                    <Text>{trail.description}</Text>
                </Card>

                <Group gap="xs">
                    <Badge color="blue">{trail.difficultyLevel}</Badge>
                    <Badge color="grape">{trail.terrainType}</Badge>
                    <Badge color="teal">{trail.surfaceType}</Badge>
                </Group>

                {trail.webUrl && (
                    <Text component="a" href={trail.webUrl} target="_blank" rel="noopener noreferrer">
                        Skoða hlaupaleiðina <IconExternalLink size={20} />
                    </Text>
                )}
            </Stack>
        </Container>
    );
}