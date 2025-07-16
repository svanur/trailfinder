// src/contexts/NavigationContext.tsx
import { createContext, useContext, useState, type ReactNode, useEffect } from 'react';
import { useLocation } from 'react-router-dom'; // Import useLocation to read current path

interface NavigationContextType {
    selectedPageName: string;
    setSelectedPageName: (name: string) => void;
}

const NavigationContext = createContext<NavigationContextType | undefined>(undefined);

// Map paths to their display names for the header
const pagePathToNameMap: { [key: string]: string } = {
    '/': 'Yfirlit',
    '/trails': 'Hlaupaleiðir',
    '/users': 'Notendur',
    '/settings': 'Stillingar Notanda', // Add this if you have a settings page in main nav
    // Add other paths as needed
};

export function NavigationProvider({ children }: { children: ReactNode }) {
    const location = useLocation();
    const [selectedPageName, setSelectedPageName] = useState('Yfirlit'); // Default value

    // Update the selectedPageName when the URL path changes (e.g., on direct navigation or refresh)
    useEffect(() => {
        const currentPath = location.pathname;
        const displayName = pagePathToNameMap[currentPath] || 'Yfirlit'; // Default if a path not found
        setSelectedPageName(displayName);
    }, [location.pathname]); // Re-run effect when location.pathname changes

    return (
        <NavigationContext.Provider value={{ selectedPageName, setSelectedPageName }}>
            {children}
        </NavigationContext.Provider>
    );
}

export function useNavigation() {
    const context = useContext(NavigationContext);
    if (context === undefined) {
        throw new Error('useNavigation must be used within a NavigationProvider');
    }
    return context;
}
