// src/components/SearchBar.tsx
import React, { useState, useEffect } from 'react';
import { Trail } from '../types';
import { Link } from 'react-router-dom';

interface SearchBarProps {
    trails: Trail[];
}

const SearchBar: React.FC<SearchBarProps> = ({ trails }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState<Trail[]>([]);
    const [isSearching, setIsSearching] = useState(false);

    useEffect(() => {
        if (searchTerm.trim() === '') {
            setSearchResults([]);
            return;
        }

        const results = trails.filter(trail =>
            trail.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            trail.description.toLowerCase().includes(searchTerm.toLowerCase())
        );
        setSearchResults(results);
    }, [searchTerm, trails]);

    return (
        <div className="relative w-full max-w-2xl">
            <div className="relative">
                <input
                    type="text"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    onFocus={() => setIsSearching(true)}
                    placeholder="Sláðu inn kennileiti..."
                    className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                />
                <svg
                    className="absolute right-3 top-2.5 h-5 w-5 text-gray-400"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                    />
                </svg>
            </div>

            {isSearching && searchTerm && (
                <div className="absolute z-10 w-full mt-1 bg-white rounded-lg shadow-lg border">
                    {searchResults.length > 0 ? (
                        <ul>
                            {searchResults.map((trail) => (
                                <li key={trail.id}>
                                    <Link
                                        to={`/run/${trail.normalizedName}`}
                                        className="block px-4 py-2 hover:bg-gray-100"
                                        onClick={() => {
                                            setSearchTerm('');
                                            setIsSearching(false);
                                        }}
                                    >
                                        <div className="font-medium">{trail.name}</div>
                                        <div className="text-sm text-gray-600">
                                            {trail.distanceKm.toFixed(1)} km • {trail.elevationGainMeters}m elevation
                                        </div>
                                    </Link>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <div className="px-4 py-2 text-gray-500">Ekkert fannst, reyndu aftur...</div>
                    )}
                </div>
            )}

            {isSearching && (
                <div
                    className="fixed inset-0 z-0"
                    onClick={() => setIsSearching(false)}
                ></div>
            )}
        </div>
    );
};

export default SearchBar;