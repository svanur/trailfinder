'use client';

import { TrailForm } from '../TrailForm';
import { useParams, useNavigate } from 'react-router-dom';
import { Container } from '@mantine/core';

export function EditTrailPage() {
    const { trailId } = useParams();
    const navigate = useNavigate();

    const handleSuccess = () => {
        // Redirect to the trails list page or dashboard after a successful submission
        navigate('/trails');
    };

    if (!trailId) {
        return <Container>Engin hlaupaleið fannst.</Container>;
    }

    return (
        <Container size="md">
            <TrailForm trailId={trailId} onSuccess={handleSuccess} />
        </Container>
    );
}
