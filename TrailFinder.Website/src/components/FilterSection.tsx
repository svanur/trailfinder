// components/FilterSection.tsx
import { Group, Select, RangeSlider, Box, Text } from '@mantine/core';

export function FilterSection() {
    return (
        <Box my="lg">
            <Group grow>
                <Select
                    label="Landshluti"
                    placeholder="Veldu landshluta"
                    data={[
                        'Höfuðborgarsvæðið',
                        'Suðurland',
                        'Norðurland',
                        'Austurland',
                        'Vesturland'
                    ]}
                />

                <Select
                    label="Erfiðleikastig"
                    placeholder="Veldu erfiðleikastig"
                    data={[
                        'Létt',
                        'Miðlungs',
                        'Erfitt'
                    ]}
                />

                <Select
                    label="Undirlag"
                    placeholder="Veldu undirlag"
                    data={[
                        'Malbik',
                        'Möl',
                        'Stígur',
                        'Fjallaleið'
                    ]}
                />
            </Group>

            <Box mt="md">
                <Text size="sm">Vegalengd (km)</Text>
                <RangeSlider
                    min={0}
                    max={50}
                    defaultValue={[0, 20]}
                    marks={[
                        { value: 0, label: '0' },
                        { value: 25, label: '25' },
                        { value: 50, label: '50' }
                    ]}
                />
            </Box>
        </Box>
    );
}