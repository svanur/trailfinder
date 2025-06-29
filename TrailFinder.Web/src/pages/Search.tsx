// src/pages/Search.tsx
import React, { useState, useMemo } from 'react';
import Layout from '../components/layout/Layout';
import TrailCard from '../components/trails/TrailCard';
import { useTrails } from '../hooks/useTrails.ts';

interface FilterState {
    searchTerm: string;
    minDistance: string;
    maxDistance: string;
    minElevation: string;
    maxElevation: string;
}

const Search: React.FC = () => {
    
    const [filters, setFilters] = useState<FilterState>({
        searchTerm: '',
        minDistance: '',
        maxDistance: '',
        minElevation: '',
        maxElevation: ''
    });

    const { data: trails, isLoading, error } = useTrails();

    if (isLoading) {
        return <div>Loading trails...</div>;
    }

    if (error) {
        return <div>Ekki tókst að hlaða upp hlaupaleiðum :(</div>;
    }

    const filteredTrails = useMemo(() => {
        // If the 'trails' variable is undefined, return an empty array
        if (!trails)
        {
            return [];   
        }

        return trails.filter(trail => {
            const matchesSearch = !filters.searchTerm ||
                trail.name.toLowerCase().includes(filters.searchTerm.toLowerCase()) ||
                trail.description.toLowerCase().includes(filters.searchTerm.toLowerCase());

            const matchesDistance = (
                (!filters.minDistance || trail.distance >= parseFloat(filters.minDistance)) &&
                (!filters.maxDistance || trail.distance <= parseFloat(filters.maxDistance))
            );

            const matchesElevation = (
                (!filters.minElevation || trail.elevation_gain >= parseFloat(filters.minElevation)) &&
                (!filters.maxElevation || trail.elevation_gain <= parseFloat(filters.maxElevation))
            );

            return matchesSearch && matchesDistance && matchesElevation;
        });
    }, [filters, trails]); // Added trails to dependencies


    const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFilters(prev => ({
            ...prev,
            [name]: value
        }));
    };

    return (
        <Layout>
            <div className="container mx-auto p-4">
                <h1 className="text-3xl font-bold mb-6">Leita að hlaupaleiðum</h1>
                
                {/* Search Filters */}
                <div className="bg-white p-6 rounded-lg shadow-md mb-8">
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {/* Text Search */}
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-2">
                                Leitarorð
                            </label>
                            <input
                                type="text"
                                name="searchTerm"
                                value={filters.searchTerm}
                                onChange={handleFilterChange}
                                className="w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                placeholder="Leita að nafni eða lýsingu..."
                            />
                        </div>

                        {/* Distance Range */}
                        <div className="space-y-2">
                            <label className="block text-sm font-medium text-gray-700">
                                Vegalengd (km)
                            </label>
                            <div className="flex gap-2">
                                <input
                                    type="number"
                                    name="minDistance"
                                    value={filters.minDistance}
                                    onChange={handleFilterChange}
                                    className="w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500"
                                    placeholder="Lágmark"
                                    min="0"
                                />
                                <input
                                    type="number"
                                    name="maxDistance"
                                    value={filters.maxDistance}
                                    onChange={handleFilterChange}
                                    className="w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500"
                                    placeholder="Hámark"
                                    min="0"
                                />
                            </div>
                        </div>

                        {/* Elevation Range */}
                        <div className="space-y-2">
                            <label className="block text-sm font-medium text-gray-700">
                                Hækkun (m)
                            </label>
                            <div className="flex gap-2">
                                <input
                                    type="number"
                                    name="minElevation"
                                    value={filters.minElevation}
                                    onChange={handleFilterChange}
                                    className="w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500"
                                    placeholder="Lágmark"
                                    min="0"
                                    step="10"
                                />
                                <input
                                    type="number"
                                    name="maxElevation"
                                    value={filters.maxElevation}
                                    onChange={handleFilterChange}
                                    className="w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500"
                                    placeholder="Hámark"
                                    min="0"
                                    step="10"
                                />
                            </div>
                        </div>
                    </div>
                </div>

                {/* Results */}
                <div className="space-y-4">
                    <div className="flex justify-between items-center mb-4">
                        <h2 className="text-xl font-semibold">
                            Niðurstöður ({filteredTrails.length})
                        </h2>
                    </div>
                    {filteredTrails.length > 0 ? (
                        filteredTrails.map(trail => (
                            <TrailCard key={trail.id} trail={trail} />
                        ))
                    ) : (
                        <p className="text-gray-500">Engar hlaupaleiðir fundust með þessum skilyrðum.</p>
                    )}
                </div>
            </div>
        </Layout>
    );
};

export default Search;