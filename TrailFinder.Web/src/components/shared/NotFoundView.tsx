// src/components/shared/NotFoundView.tsx
import React from 'react';
import Layout from '../layout/Layout';

interface NotFoundViewProps {
    message: string;
}

const NotFoundView: React.FC<NotFoundViewProps> = ({ message }) => (
    <Layout>
        <div>{message}</div>
    </Layout>
);

export default NotFoundView;
