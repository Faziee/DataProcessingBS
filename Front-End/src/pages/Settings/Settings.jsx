import React, { useState } from 'react';
import './Settings.css';
import Navbar from './Navbar'; // Assuming you have a Navbar component

const Settings = ({ user, updateUser }) => {
    const [formData, setFormData] = useState({
        email: user?.email || '',
        password: '',
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        updateUser(formData);
    };

    return (
        <>
            <Navbar />
            <div className="settings-container">
                <h1>Account Settings</h1>
                <form className="settings-form" onSubmit={handleSubmit}>
                    <div className="form-field">
                        <label>Email:</label>
                        <input
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleInputChange}
                        />
                    </div>
                    <div className="form-field">
                        <label>New Password:</label>
                        <input
                            type="password"
                            name="password"
                            value={formData.password}
                            onChange={handleInputChange}
                        />
                    </div>
                    <button type="submit">Update Account</button>
                </form>
            </div>
        </>
    );
};

export default Settings;
