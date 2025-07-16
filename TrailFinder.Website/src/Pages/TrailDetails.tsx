// src/pages/TrailDetails.tsx
import { useParams } from 'react-router-dom';
import { 
    Container, 
    Title 
} from '@mantine/core';

export function TrailDetails() {
    const { slug } = useParams();

    // Hér kemur lógíkin fyrir trail details síðuna
    return (
        <Container>
            <Title>Hlaupaleið {slug}</Title>
            {/* Hér kemur innihald síðunnar */}
        </Container>
    );
}