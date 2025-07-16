// components/Routes/RouteCard.tsx
import { Card, Image, Text, Badge, Group, Stack, ActionIcon } from '@mantine/core';
import { IconHeart } from '@tabler/icons-react';
import type { Route } from '../../types/types';

interface RouteCardProps {
    route: Route;
    onFavoriteClick: (routeId: string) => void;
}

export function RouteCard({ route, onFavoriteClick }: RouteCardProps) {
    return (
        <Card shadow="sm" padding="lg" radius="md" withBorder>
            {route.imageUrl && (
                <Card.Section>
                    <Image
                        src={route.imageUrl}
                        height={160}
                        alt={route.name}
                    />
                </Card.Section>
            )}

            <Stack gap="xs" mt="md">
                <Group justify="apart">
                    <Text fw={500} size="lg">{route.name}</Text>
                    <Badge variant="light">{route.difficulty}</Badge>
                </Group>

                <Group gap="xs">
                    <Text size="sm" c="dimmed">
                        {route.distance}km
                    </Text>
                    <Text size="sm" c="dimmed">•</Text>
                    <Text size="sm" c="dimmed">
                        {route.region}
                    </Text>
                </Group>

                <Text size="sm" c="dimmed" lineClamp={2}>
                    {route.description}
                </Text>

                <Group justify="space-between" mt="md">
                    <Group gap="xs">
                        <Badge variant="light" color="blue">
                            {route.terrainType}
                        </Badge>
                        {route.elevation && (
                            <Badge variant="light" color="grape">
                                {route.elevation.gain}m hækkun
                            </Badge>
                        )}
                    </Group>
                    <ActionIcon
                        variant="subtle"
                        color="gray"
                        onClick={() => onFavoriteClick(route.id)}
                    >
                        <IconHeart style={{ width: '70%', height: '70%' }} />
                    </ActionIcon>
                </Group>
            </Stack>
        </Card>
    );
}
