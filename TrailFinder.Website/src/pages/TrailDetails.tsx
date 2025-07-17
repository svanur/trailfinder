// src/pages/TrailDetails.tsx (No changes needed here for routing)
import { useParams } from 'react-router-dom';
import { useTrail } from '../hooks/useTrail';
import { Container, Title, Text, Group, Stack, Badge, Loader, Card } from '@mantine/core';
import { /*IconMapPin,*/ IconRuler, IconMountain, IconRoute } from '@tabler/icons-react';

export function TrailDetails() {
    const { slug } = useParams<{ slug: string }>();
    const { data: trail, isLoading, error } = useTrail(slug ?? '');

    if (isLoading) {
        return (
            <Container>
                <Loader size="xl" />
            </Container>
        );
    }

    if (error || !trail) {
        return (
            <Container>
                <Text color="red">Villa kom upp við að sækja upplýsingar um hlaupaleiðina</Text>
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

                {/*{trail.trailLocations?.length > 0 && (
                    <Card withBorder>
                        <Text size="lg" fw={500} mb="md">Staðsetningar</Text>
                        <Stack gap="xs">
                            {trail.trailLocations.map((location) => (
                                <Group key={location.id}>
                                    <IconMapPin size={16} />
                                    <Text>{location.name}</Text>
                                </Group>
                            ))}
                        </Stack>
                    </Card>
                )}*/}

                {trail.webUrl && (
                    <Text component="a" href={trail.webUrl} target="_blank" rel="noopener noreferrer">
                        Nánar um hlaupaleiðina
                    </Text>
                )}
            </Stack>
        </Container>
    );
}
