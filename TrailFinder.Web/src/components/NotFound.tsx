// src/components/NotFound.tsx
import React, { useMemo } from 'react';
import { Link } from 'react-router-dom';
import Layout from './layout/Layout';

const notFoundMessages = [
    "Oops, you're off track! 🗺️",
    "Oh oh, are you lost? 🧭",
    "This trail leads to nowhere! 🌲",
    "Looks like you took a wrong turn at the last cairn! ⛰️",
    "Trail not found - maybe the trolls moved it? 🧌",
    "This path is less traveled... because it doesn't exist! 🌿",
    "Even GPS can't find this route! 📍",
    "Somewhere between here and there... but mostly nowhere! 🚶‍♂️",
    "404 meters above confusion level! 🏔️",
    "You've wandered into uncharted territory! 🗺️"
];

const NotFound: React.FC = () => {
    const randomMessage = useMemo(() => {
        const randomIndex = Math.floor(Math.random() * notFoundMessages.length);
        return notFoundMessages[randomIndex];
    }, []);

    return (
        <Layout>
            <div className="container mx-auto px-4 py-16">
                <div className="text-center">
                    <h1 className="text-6xl font-bold text-gray-800 mb-4">404</h1>
                    <p className="text-2xl mb-8 text-gray-600">{randomMessage}</p>
                    <Link
                        to="/"
                        className="inline-flex items-center px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                    >
                        <svg
                            className="w-5 h-5 mr-2"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
                            />
                        </svg>
                        Aftur á upphafsreit 
                    </Link>
                </div>
            </div>
        </Layout>
    );
};

export default NotFound;