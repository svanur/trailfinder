import React from 'react';
import {Link} from 'react-router-dom';
import Layout from "../layout/Layout.tsx";

interface ErrorViewProps {
    message?: string;
}

const ErrorView: React.FC<ErrorViewProps> = ({ message }) => {
    return (

        <Layout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] mt-16">
                <div className="p-8 rounded-lg border-2 border-red-200 bg-white/50 backdrop-blur-sm shadow-lg">
                    <div className="relative mb-6 flex justify-center">
                    <span className="material-symbols-outlined text-7xl text-red-600">
                        falling
                    </span>
                    </div>
                    <p className="text-xl font-semibold text-gray-700 mb-3 text-center">
                        {/*Oops! We took a tumble*/}
                        Ahh, datt á hausinn..
                    </p>
                    {message && (
                        <p className="text-sm text-red-600 mb-6 text-center p-3 bg-red-50 rounded-md">
                            {message}
                        </p>
                    )}
                    <div className="flex justify-center">
                        <Link
                            to="/"
                            className="inline-block px-6 py-2 bg-red-400 text-white rounded-lg hover:bg-red-700 transition-colors"
                        >
                            Til baka á byrjunarreit...
                            {/*Back to Safe Trail*/}
                        </Link>
                    </div>
                </div>
            </div>
        </Layout>
    );
};

export default ErrorView;