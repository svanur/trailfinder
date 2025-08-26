import React, {useEffect, useState} from 'react';
import Layout from "../layout/Layout.tsx";

const LoadingView: React.FC = () => {
    const warmupActivities = ['Sveifla fótum', 'Hoppa', 'Teygja', 'Rólegt skokk', 'Anda'];
    const [currentActivity, setCurrentActivity] = useState(0);

    useEffect(() => {
        const interval = setInterval(() => {
            setCurrentActivity((current) => (current + 1) % warmupActivities.length);
        }, 2000);

        return () => clearInterval(interval);
    }, []);

    return (
        <Layout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] mt-16">
                <div className="p-8 rounded-lg border-2 border-gray-200 bg-white/50 backdrop-blur-sm shadow-lg">
                    <div className="relative mb-6 flex justify-center">
                    <span
                        className="material-symbols-outlined text-6xl text-gray-500"
                        style={{
                            display: 'inline-block',
                        }}
                    >
                        directions_run
                    </span>
                    </div>
                    <p className="text-xl font-semibold text-gray-700 mb-3">
                        Ævintýrin eru á næsta leyti #partyon
                        {/*Loading your trail adventure...*/}
                    </p>
                    <p className="text-sm text-gray-500">
                        Hitum upp á meðan: <span className="font-medium">{warmupActivities[currentActivity]}</span>...
                    </p>
                </div>
            </div>
        </Layout>
    );
};

export default LoadingView;