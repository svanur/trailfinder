import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import TrailDetails from './pages/TrailDetails';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/run/:normalizedName" element={<TrailDetails />} />
            </Routes>
        </Router>
    );
}

export default App;