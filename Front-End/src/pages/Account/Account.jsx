import NavBar from '../../components/Navbar/Navbar.jsx';
import React, { useState } from "react";
import TitleCard from "../../components/TitleCards/TitleCard.jsx";
import './Account.css';

const Account = () => {

    const [searchQuery, setSearchQuery] = useState('');

    const handleSearchChange = (e) => {
        setSearchQuery(e.target.value);
    };

    const handleSearch = () => {
        console.log('Search for:', searchQuery);
    };

    return (
        <div className="account-page">
            <NavBar />
            <div className="account-container">
                <h1>Account Information</h1>
                <div className="info-section">
                    <div className="info-field">
                        <label>Name:</label>
                        <p></p>
                    </div>
                    <div className="info-field">
                        <label>Email:</label>
                        <p></p>
                    </div>
                    <div className="info-field">
                        <label>Phone Number:</label>
                        <p></p>
                    </div>
                    <div className="info-field">
                        <label>API Key:</label>
                        <p></p>
                    </div>
                </div>
            </div>
            <div className="search-section">
                <div className="search-container">
                    <input
                        type="text"
                        value={searchQuery}
                        onChange={handleSearchChange}
                        placeholder="Search by genre..."
                        className="search-input"
                    />
                    <button onClick={handleSearch} className="search-button">
                        GET
                    </button>
                </div>
                <TitleCard title="Movies by Genre" category={searchQuery} />
            </div>
        </div>
    );
}

export default Account;
