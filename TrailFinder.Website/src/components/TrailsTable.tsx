// TrailsTable.tsx
import { Table, Text } from '@mantine/core';
import { NavLink as MantineNavLink } from '@mantine/core'; // Alias Mantine's NavLink
import { NavLink as RouterNavLink } from 'react-router-dom'; // Import react-router-dom's NavLink

import { useTrails } from '../hooks/useTrails';
import { IconActivity } from "@tabler/icons-react";

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
                {/* Use MantineNavLink with component prop for router integration */}
                <MantineNavLink
                    component={RouterNavLink} // Tell MantineNavLink to render as RouterNavLink
                    to={`/hlaup/${trail.slug}`} // Use 'to' prop for react-router-dom
                    label={trail.name}
                    description={trail.description}
                    leftSection={<IconActivity size={16} stroke={1.5} />}
                />
            </Table.Td>
            <Table.Td>{trail.distanceMeters}</Table.Td>
            <Table.Td>{trail.elevationGainMeters}</Table.Td>
            <Table.Td>{trail.surfaceType}</Table.Td>
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
                    <Table.Th>Yfirborð</Table.Th>
                    <Table.Th>Erfiðleiki</Table.Th>
                    <Table.Th>Tegund</Table.Th>
                    <Table.Th>Landslag</Table.Th>
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
            <Table.Caption>Hlaupaleiðir</Table.Caption>
        </Table>
    );
}
