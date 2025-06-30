// src/pages/RouteDetails.tsx
import { useParams } from 'react-router-dom';
import { 
    Container, 
    Title 
} from '@mantine/core';

export function RouteDetails() {
    const { id } = useParams();

    // Hér kemur lógíkin fyrir route details síðuna
    return (
        <Container>
            <Title>Hlaupaleið {id}</Title>
            {/* Hér kemur innihald síðunnar */}
        </Container>
    );
}