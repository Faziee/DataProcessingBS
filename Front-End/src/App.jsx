import React from 'react';
import { Routes, Route } from 'react-router-dom';
import PrivateRoute from './components/PrivateRoute/PrivateRoute';
import Home from './pages/Home/Home';
import Login from './pages/Login/Login';
import Player from './pages/Player/Player';
import API from './pages/API/API';
import Settings from './pages/Settings/Settings';

const App = () => {
    return (
        <Routes>
            {/* Public Route */}
            <Route path="/login" element={<Login />} />

            {/* Protected Route */}
            <Route
                path="/home"
                element={
                    <PrivateRoute>
                        <Home />
                    </PrivateRoute>
                }
            />
            <Route path="/player" element={<Player />} />
            <Route path="/api" element={<API />} />
            <Route path="/settings" element={<Settings />} />
        </Routes>
    );
};

export default App;
