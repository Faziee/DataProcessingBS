import React from 'react';
import { Routes, Route } from "react-router-dom";
import PrivateRoute from '../../components/PrivateRoute/PrivateRoute.jsx';
import Home from '../../pages/Home/Home.jsx';
import Login from "../../pages/Login/Login";
import Player from "../../pages/Player/Player";
import API from "../../pages/API/API";
import Settings from "../../pages/Settings/Settings";

const App = () => {
    return (
        <div>
            <Routes>
                {/* Login route is public */}
                <Route path="/login" element={<Login />} />

                {/* Protected routes wrapped with PrivateRoute */}
                <Route
                    path="/"
                    element={
                        <PrivateRoute>
                            <Home />
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/player/:id"
                    element={
                        <PrivateRoute>
                            <Player />
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/API"
                    element={
                        <PrivateRoute>
                            <API />
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/Settings"
                    element={
                        <PrivateRoute>
                            <Settings />
                        </PrivateRoute>
                    }
                />
            </Routes>
        </div>
    );
};

export default App;