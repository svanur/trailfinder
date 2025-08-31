// src/pages/Trails.tsx

'use client';

import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
    Table,
    Badge,
    Card,
    Text,
    ActionIcon,
    Group,
    Button,
    Modal,
    Title,
    Box,
    LoadingOverlay,
    Container
} from '@mantine/core';
import { IconEdit, IconTrash } from '@tabler/icons-react';
import { Link } from 'react-router-dom';
import { trailsApi } from '../../services/trailsApi';
import { Loading } from '../Loading';
import { IconEye, IconEyeClosed } from '@tabler/icons-react';
import {type Trail } from '@trailfinder/db-types';

const getDifficultyColor = (difficulty: Trail['difficultyLevel']) => {
    switch (difficulty) {
        case 'easy':
            return 'green';
        case 'moderate':
            return 'yellow';
        case 'hard':
            return 'orange';
        case 'extreme':
            return 'red';
        default:
            return 'gray';
    }
};

export function Trails() {
    const queryClient = useQueryClient();
    const [opened, setOpened] = useState(false);
    const [trailToDelete, setTrailToDelete] = useState<Trail | null>(null);

    // FIX: Explicitly type the useQuery hook
    const { data: allTrails, isLoading, error } = useQuery<Trail[]>({
        queryKey: ['all-trails'],
        queryFn: () => trailsApi.getAll(),
    });

    const deleteMutation = useMutation({
        mutationFn: (trailId: string) => trailsApi.delete(trailId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['all-trails'] });
            setOpened(false); // Close the modal
        },
        onError: (e) => {
            console.error('Failed to delete trail:', e);
        }
    });

    const confirmDelete = (trail: Trail) => {
        setTrailToDelete(trail);
        setOpened(true);
    };

    const handleDelete = () => {
        if (trailToDelete) {
            deleteMutation.mutate(trailToDelete.id);
        }
    };

    // FIX: Reset modal state on close
    const closeModal = () => {
        setOpened(false);
        setTrailToDelete(null);
    };

    if (isLoading) {
        return <Loading text="Loading all trails..." />;
    }

    if (error) {
        return <Text c="red">Could not load all trails: {error.message}</Text>;
    }

    const activeTrails = allTrails?.filter(trail => trail.isActive);

    return (
        <Container size="xl">
            <Group justify="space-between" mb="lg">
                <Title order={2}>Trail Management</Title>
                <Button component={Link} to="/trails/new">
                    Skrá nýja hlaupaleið
                </Button>
            </Group>

            <Box pos="relative">
                <LoadingOverlay visible={deleteMutation.isPending} zIndex={1000} overlayProps={{ radius: 'sm', blur: 2 }} />
                {activeTrails && activeTrails.length > 0 ? (
                    <Card withBorder>
                        <Table highlightOnHover>
                            <Table.Thead>
                                <Table.Tr>
                                    <Table.Th>Nafn</Table.Th>
                                    <Table.Th>Vegalengd</Table.Th>
                                    <Table.Th>Hækkun</Table.Th>
                                    <Table.Th>Lækkun</Table.Th>
                                    <Table.Th>Erfiðleikastig</Table.Th>
                                    <Table.Th>Birta á vef</Table.Th>
                                    <Table.Th>Aðgerðir</Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {activeTrails.map((trail) => (
                                    <Table.Tr key={trail.id}>
                                        <Table.Td>{trail.name}</Table.Td>
                                        <Table.Td>{trail.distanceMeters} m</Table.Td>
                                        <Table.Td>{trail.elevationGainMeters} m</Table.Td>
                                        <Table.Td>{trail.elevationLossMeters} m</Table.Td>
                                        <Table.Td>
                                            <Badge color={getDifficultyColor(trail.difficultyLevel)}>
                                                {trail.difficultyLevel}
                                            </Badge>
                                        </Table.Td>
                                        <Table.Td>
                                                {trail.isActive
                                                    ? <IconEye />
                                                    : <IconEyeClosed />
                                                }
                                        </Table.Td>
                                        <Table.Td>
                                            <Group gap="xs" wrap="nowrap">
                                                <ActionIcon
                                                    variant="light"
                                                    color="blue"
                                                    component={Link}
                                                    to={`/trails/${trail.id}/edit`}
                                                    aria-label={`Breyta ${trail.name}`}
                                                >
                                                    <IconEdit style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                                </ActionIcon>
                                                <ActionIcon
                                                    variant="light"
                                                    color="red"
                                                    onClick={() => confirmDelete(trail)}
                                                    aria-label={`Eyða ${trail.name}`}
                                                >
                                                    <IconTrash style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                                </ActionIcon>
                                            </Group>
                                        </Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Card>
                ) : (
                    <Text c="dimmed">No trails found.</Text>
                )}
            </Box>

            {/* Confirmation Modal for Deletion */}
            <Modal opened={opened} onClose={closeModal} title="Confirm Deletion">
                <Text>Are you sure you want to delete **{trailToDelete?.name}**?</Text>
                <Group justify="flex-end" mt="md">
                    <Button variant="outline" onClick={closeModal}>Cancel</Button>
                    <Button color="red" onClick={handleDelete}>Delete</Button>
                </Group>
            </Modal>
        </Container>
    );
}
