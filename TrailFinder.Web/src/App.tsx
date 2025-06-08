// src/App.tsx
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import Home from './pages/Home';
import TrailDetails from './pages/TrailDetails';
import NotFound from './components/NotFound';
import Search from './pages/Search';

const queryClient = new QueryClient();

const App: React.FC = () => {
    return (
        <QueryClientProvider client={queryClient}>
            <Router>
                <Routes>
                    <Route path="/" element={<Home/>}/>
                    <Route path="/run/:slug" element={<TrailDetails/>}/>
                    <Route path="/search" element={<Search/>}/>
                    <Route path="*" element={<NotFound/>}/>
                </Routes>
            </Router>
        </QueryClientProvider>
    );
};

export default App;
