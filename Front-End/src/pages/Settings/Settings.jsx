// Modified Settings.jsx
import React, { useState, useEffect } from 'react';
import './Settings.css';
import NavBar from '../../components/Navbar/Navbar';

const Settings = () => {
    const [account, setAccount] = useState(null);

    useEffect(() => {
        const userData = JSON.parse(localStorage.getItem('user'));
        setAccount(userData);
    }, []);

    return (
        <div className="account-page">
            <NavBar />
            <div className="account-container">
                <h1>Account Information</h1>
                <div className="info-section">
                    <div className="info-field">
                        <label>Email:</label>
                        <p>{account?.email || 'N/A'}</p>
                    </div>
                    <div className="info-field">
                        <label>Payment Method:</label>
                        <p>{account?.payment_Method || 'N/A'}</p>
                    </div>
                    <div className="info-field">
                        <label>Trial End Date:</label>
                        <p>{account?.trial_End_Date || 'N/A'}</p>
                    </div>
                    <div className="info-field">
                        <label>API Key:</label>
                        <p>{localStorage.getItem('apiKey') || 'N/A'}</p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Settings;