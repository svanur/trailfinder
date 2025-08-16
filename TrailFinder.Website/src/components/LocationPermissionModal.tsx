// TrailFinder.Website\src\components\LocationPermissionModal.tsx
import {Modal, Stack, Title, Text, Button, Center, Divider} from '@mantine/core';
import { IconRefresh } from '@tabler/icons-react';

interface LocationPermissionModalProps {
    opened: boolean;
    onClose: () => void;
}

export function LocationPermissionModal({ opened, onClose }: LocationPermissionModalProps) {
    return (
        <Modal
            opened={opened}
            centered
            onClose={onClose}
            styles={{
                header: {
                    backgroundColor: 'var(--mantine-color-blue-light)', // Keep the header background
                    paddingTop: 'var(--mantine-spacing-md)',
                    paddingBottom: 'var(--mantine-spacing-md)',
                    borderBottom: '1px solid var(--mantine-color-gray-2)', // Keep the border
                },
                title: {
                    color: 'var(--mantine-color-blue-filled)',
                    fontWeight: 'bold',
                },
                content: { // Keep content as flex column for consistent layout
                    display: 'flex',
                    flexDirection: 'column',
                    maxHeight: '80vh', // Adjust modal's overall max height as needed
                },
                body: { // This will now naturally scroll if content overflows maxHeight
                    flexGrow: 1,
                    overflowY: 'auto',
                    // Remove paddingBottom that was for the sticky footer
                    paddingBottom: '0', // Or var(--mantine-spacing-md) for standard spacing
                },
            }}
            title="Af hverju biðjum við um staðsetningu?"
        >
            {/* The content that should scroll, now including the button */}
            <Stack gap="md" style={{ padding: 'var(--mantine-spacing-md)' }}>
                <Title order={5}>Til að bæta upplifun þína á vefnum okkar</Title>
                
                <Text>
                    Staðsetning er m.a. notuð til að:
                </Text>
                <ul>
                    <li><strong>Finna nálægar leiðir</strong>
                        <Text>Með staðsetningu getum við birt lista, þar sem fyrstu leiðirnar eru næst þinni staðsetningu</Text>
                    </li>
                    <li>
                        <strong>Staðsetning á korti</strong>
                        <Text>Með staðsetningu getum birt hvar þú ert, þá sérðu nálægar leiðir</Text>
                        <Text>Við gætum hjálpað þér að finna akstursleið frá þinni staðsetningu að byrjun leiðar</Text>
                    </li>
                </ul>
                <Text>
                    <strong>Hvernig við notum staðsetninguna:</strong>
                </Text>
                <ul>
                    <li>Staðsetningin er aðeins notuð til að framkvæma leit, raða leiðum o.þ.h.</li>
                    <li>Staðsetning þín er <u>aldrei vistuð</u> af okkur</li>
                    <li>Þú getur alltaf afturkallað leyfið í stillingum vafrans</li>
                </ul>

                <Divider my="md" />
                
                <Text>
                    <strong>Smelltu á <IconRefresh size={16} stroke={1.5} /> táknið til að fá staðsetninguna</strong>
                </Text>
                <Text>
                    Ef ekkert gerist, þá getur verið að það sé alveg slökkt á staðsetningarleyfinu. 
                    Þú þarft að fara í stillingar vafrans og virkja staðsetningarleyfið.
                </Text>

                <Center mt="md">
                    <Button onClick={onClose} fullWidth>Loka</Button>
                </Center>
            </Stack>

            {/* Remove the separate sticky Box as it's no longer needed */}
        </Modal>
    );
}