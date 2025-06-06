import { Link } from 'react-router-dom';

const Header = () => {
    return (
        <header className="bg-white shadow-sm">
            <div className="container mx-auto px-4 py-4">
                <nav className="flex justify-between items-center">
                    <Link to="/" className="text-xl font-bold">
                        Hlaupaleiðir
                    </Link>
                    <div className="space-x-4">
                        <Link to="/" className="hover:text-blue-600">Heim</Link>
                        <Link to="/search" className="hover:text-blue-600">Leita</Link>
                    </div>
                </nav>
            </div>
        </header>
    );
};

export default Header;