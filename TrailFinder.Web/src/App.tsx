// src/App.tsx or wherever your router is configured
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import TrailDetails from './pages/TrailDetails';
import NotFound from './components/NotFound';
import Search from './pages/Search';


const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/run/:normalizedName" element={<TrailDetails />} />
                <Route path="*" element={<NotFound />} />
                <Route path="/search" element={<Search />} />
            </Routes>
        </Router>
    );
};

export default App;