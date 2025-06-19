import React from 'react';
import {Link} from 'react-router-dom';
import Layout from "../layout/Layout.tsx";

interface NotFoundViewProps {
    message?: string;
}

const NotFoundView: React.FC<NotFoundViewProps> = ({ message }) => {
    return (
        <Layout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] mt-16">
                <div className="p-8 rounded-lg border-2 border-blue-200 bg-white/50 backdrop-blur-sm shadow-lg">
                    <div className="relative mb-6 flex justify-center">
                        <div className="relative">
                        <span
                            className="material-symbols-outlined text-6xl text-blue-600"
                            style={{
                                display: 'inline-block',
                            }}
                        >
                            wrong_location
                        </span>
                            <span
                                className="material-symbols-outlined text-6xl text-blue-500 absolute -right-8 -bottom-4"
                            >
                            travel_explore
                        </span>
                        </div>
                    </div>
                    <p className="text-xl font-semibold text-gray-700 mb-3 text-center">
                        Bíddu, erum við bara villt!
                        {/*Looks like we've lost the trail...*/}
                    </p>
                    {message && (
                        <p className="text-sm text-red-600 mb-6 text-center p-3 bg-red-50 rounded-md">
                            {message}
                        </p>
                    )}
                    <p className="text-sm text-gray-500 mb-6 text-center">
                        Leiðin sem þú ert að leita að er ekki til, farin...
                        {/*The trail you're looking for doesn't exist or has been moved ...sort of*/}
                    </p>
                    <div className="flex justify-center">
                        <Link
                            to="/"
                            className="inline-block px-6 py-2 bg-blue-400 text-white rounded-lg hover:bg-blue-700 transition-colors"
                        >
                            Back to Main Trail
                        </Link>
                    </div>
                </div>
            </div>
        </Layout>
    );
};

export default NotFoundView;