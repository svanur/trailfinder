import React from 'react';
import Layout from '../components/layout/Layout';
import TrailCard from '../components/trails/TrailCard';
import SearchBar from "../components/SearchBar.tsx";
import { useTrails } from '../hooks/useTrails.ts';
import LoadingView from '../components/shared/LoadingView.tsx';
import ErrorView from "../components/shared/ErrorView.tsx";

const Home: React.FC = () => {

    const { data: trails, isLoading, error } = useTrails();

    if (isLoading) {
        return <LoadingView />;
    }

    if (error) {
        return <ErrorView message={`Það er eitthvað rangt við þessar laiðir: ${error.message}`} />;
    }

    if (!trails) {
        return <LoadingView />;
    }


    return (
        <Layout>
            <div className="container mx-auto p-4">
                <section className="mb-12">
                    <h1 className="text-4xl font-bold mb-2">Hlaupaleiðir á Íslandi</h1>
                    <p className="text-xl text-gray-600">Hér finnur þú áhugaverðar hlaupaleiðir</p>
                    <SearchBar trails={trails} />
                </section>

                <section>
                    <h2 className="text-2xl font-bold mb-6">Vinsælar hlaupaleiðir</h2>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {trails.map(trail => (
                            <TrailCard
                                key={trail.id}
                                trail={trail}
                            />
                        ))}
                    </div>
                </section>
            </div>
        </Layout>
    );
};

export default Home;