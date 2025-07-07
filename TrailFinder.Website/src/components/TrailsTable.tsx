import {NavLink, Table, Text} from '@mantine/core';

import { useTrails } from '../hooks/useTrails';
import {IconActivity} from "@tabler/icons-react";

export function TrailsTable() {
    const { data: trails, isLoading, error } = useTrails();

    if (isLoading) {
        return <Text>Hleð inn hlaupaleiðum...</Text>;
    }

    if (error) {
        return <Text color="red">Villa kom upp við að sækja hlaupaleiðir</Text>;
    }

    if (!trails?.length) {
        return <Text>Engar hlaupaleiðir fundust</Text>;
    }

    const rows = trails.map((trail) => (
        <Table.Tr key={trail.id}>
            <Table.Td>
                <NavLink
                    href={`/hlaup/${trail.slug}`}
                    label={trail.name}
                    description={trail.description}
                    leftSection={<IconActivity size={16} stroke={1.5} />}
                />
            </Table.Td>
            <Table.Td>{trail.distance}</Table.Td>
            <Table.Td>{trail.elevationGain}</Table.Td>
            <Table.Td>{trail.difficultyLevel}</Table.Td>
            <Table.Td>{trail.routeType}</Table.Td>
            <Table.Td>{trail.terrainType}</Table.Td>
        </Table.Tr>
    ));

    return (
        <Table stickyHeader striped highlightOnHover withTableBorder withColumnBorders stickyHeaderOffset={60}>
            <Table.Thead>
                <Table.Tr>
                    <Table.Th>Nafn</Table.Th>
                    <Table.Th>Vegalengd</Table.Th>
                    <Table.Th>Hækkun</Table.Th>
                    <Table.Th>Erfiðleiki</Table.Th>
                    <Table.Th>Tegund</Table.Th>
                    <Table.Th>Undirlag</Table.Th>
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
            <Table.Caption>Hlaupaleiðir</Table.Caption>
        </Table>
    );
}