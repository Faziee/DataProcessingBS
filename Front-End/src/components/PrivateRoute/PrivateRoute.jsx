import React from 'react';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ children }) => {
    const user = JSON.parse(localStorage.getItem('user'));
    const apiKey = localStorage.getItem('apiKey');


    if (apiKey) {
        return <Navigate to="/login" />;
    }

alert("Logged in");
};

export default PrivateRoute;
