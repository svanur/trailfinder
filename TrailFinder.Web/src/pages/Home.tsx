import React from 'react';
import Layout from '../components/layout/Layout';
import TrailCard from '../components/trails/TrailCard';
import { trails } from '../data';
import SearchBar from "../components/SearchBar.tsx";

const Home: React.FC = () => {
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