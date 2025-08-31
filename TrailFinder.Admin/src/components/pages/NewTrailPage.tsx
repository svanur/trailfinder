'use client';


import { useNavigate } from 'react-router-dom';
import { Container } from '@mantine/core';
import {TrailForm} from "../TrailForm.tsx";

export function NewTrailPage() {
    const navigate = useNavigate();

    const handleSuccess = () => {
        // Redirect to the trails list page or dashboard after a successful submission
        navigate('/trails');
    };

    return (
        <Container size="md">
            <TrailForm onSuccess={handleSuccess} />
        </Container>
    );
}