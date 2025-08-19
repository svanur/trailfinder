// src/components/dashboard/RecentTrailsTable.tsx
import { Table, Badge, Card, Text, ActionIcon } from '@mantine/core'; // Added ActionIcon, Group
import { useQuery } from '@tanstack/react-query';
import { supabase } from '../../lib/supabase';
import { IconEdit } from '@tabler/icons-react'; // For the Edit icon
import { Link } from 'react-router-dom';
import {Loading} from "../Loading.tsx"; // For navigation

interface Trail {
    id: string;
    name: string;
    distance_meters: number;
    elevation_gain_meters: number;
    difficulty_level: 'unknown' | 'easy' | 'moderate' | 'hard' | 'extreme';
    created_at: string;
    // route_geom: any; // Not needed for this table
}

export function RecentTrailsTable() {
    const { data: recentTrails, isLoading: recentTrailsLoading, error } = useQuery<Trail[]>({
        queryKey: ['recent-trails'],
        queryFn: async () => { //TODO: replace this supabase query with an API call
            const { data, error } = await supabase
                .from('trails')
                .select('id, name, distance_meters, elevation_gain_meters, difficulty_level, created_at')
                .order('created_at', { ascending: false })
                .limit(5);

            if (error) throw error;
            return data as Trail[];
        }
    });

    if (recentTrailsLoading) {
        return <Loading text="Augnablik meðan við hlöðum inn nýjustu hlaupaleiðunum..." />;
    }

    if (error) {
        return <Text c="red">Gat ekki hlaðið nýjustu hlaupaleiðum: {error.message}</Text>;
    }

    const getDifficultyColor = (difficulty: Trail['difficulty_level']) => {
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

    return (
        <Card withBorder>
            <Text fw={600} mb="md">Nýjustu hlaupaleiðir</Text>
            {recentTrails && recentTrails.length > 0 ? (
                <Table highlightOnHover> {/* Added highlightOnHover */}
                    <Table.Thead>
                        <Table.Tr>
                            <Table.Th>Nafn</Table.Th>
                            <Table.Th>Vegalengd</Table.Th>
                            <Table.Th>Hækkun</Table.Th>
                            <Table.Th>Erfiðleikastig</Table.Th>
                            <Table.Th>Aðgerðir</Table.Th> {/* New header for actions */}
                        </Table.Tr>
                    </Table.Thead>
                    <Table.Tbody>
                        {recentTrails.map((trail) => (
                            <Table.Tr key={trail.id}>
                                <Table.Td>{trail.name}</Table.Td>
                                <Table.Td>{trail.distance_meters} km</Table.Td>
                                <Table.Td>{trail.elevation_gain_meters}m</Table.Td>
                                <Table.Td>
                                    <Badge color={getDifficultyColor(trail.difficulty_level)}>
                                        {trail.difficulty_level}
                                    </Badge>
                                </Table.Td>
                                <Table.Td>
                                    <ActionIcon
                                        variant="light"
                                        color="blue"
                                        component={Link}
                                        to={`/trails/${trail.id}/edit`} // Link to the edit page
                                        aria-label={`Breyta ${trail.name}`}
                                    >
                                        <IconEdit style={{ width: '70%', height: '70%' }} stroke={1.5} />
                                    </ActionIcon>
                                </Table.Td>
                            </Table.Tr>
                        ))}
                    </Table.Tbody>
                </Table>
            ) : (
                <Text c="dimmed">Engar nýjar hlaupaleiðir fundust.</Text>
            )}
        </Card>
    );
}
