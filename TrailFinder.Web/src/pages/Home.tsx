import React from 'react';
import { Trail } from '../types';
import TrailCard from '../components/trails/TrailCard';

const Home: React.FC = () => {
    
    // This will be replaced with actual data fetching later
    const featuredTrails: Trail[] = [
        {
            id: '1',
            name: 'Esja upp að Steini',
            normalizedName: 'esja-upp-ad-steini',
            description: 'Hin klassíska leið upp að Steini, og niður aftur',
            distanceKm: 6.5,
            elevationGainMeters: 600,
            startLatitude: 64.2008,
            startLongitude: -21.6711,
            createdAt: new Date().toISOString(),
        },
    ];

    return (
        <div className="container mx-auto px-4">
            <div className="hero py-16">
                <h1 className="text-4xl font-bold text-center">
                    Hlaupaleiðir á Íslandi
                </h1>
                <p className="text-xl text-center mt-4">
                    Hér finnur þú fallegar og skemmtilegar hlaupaleiðir.
                </p>
            </div>

            <div className="featured-trails mt-12">
                <h2 className="text-2xl font-semibold mb-6">Vinsælar leiðir</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {featuredTrails.map(trail => (
                        <TrailCard key={trail.id} trail={trail} />
                    ))}
                </div>
            </div>
        </div>
    );
};

export default Home;