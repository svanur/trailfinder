// src/components/shared/ErrorView.tsx
import React from 'react';
import Layout from '../layout/Layout';

interface ErrorViewProps {
    message: string;
}

const ErrorView: React.FC<ErrorViewProps> = ({ message }) => (
    <Layout>
        <div>{message}</div>
    </Layout>
);

export default ErrorView;
