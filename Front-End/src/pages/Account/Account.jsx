import NavBar from '../../components/Navbar/Navbar.jsx';
import React, { useState, useEffect } from "react";
import TitleCard from "../../components/TitleCards/TitleCard.jsx";
import './Account.css';

const Account = () => {
    const [email, setEmail] = useState("");         // Store the email entered by the user
    const [account, setAccount] = useState(null);   // Store account data returned by the API
    const [error, setError] = useState(null);       // Store any errors from the API
    const [searchQuery, setSearchQuery] = useState('');  // For search by genre

    // Fetch account data based on email
    useEffect(() => {
        if (email) {
            // Function to fetch account by email from the API
            const getAccount = async () => {
                try {
                    const response = await fetch(`http://localhost:5025/api/account/email/${email}`);
                    if (!response.ok) {
                        throw new Error("Account not found or another error occurred");
                    }

                    const data = await response.json();
                    setAccount(data);
                } catch (err) {
                    setError(err.message);
                }
            };

            getAccount();
        }
    }, [email]);

    // Handle search query change
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
                        <label>Email:</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}  // Update email on user input
                            placeholder="Enter email"
                        />
                    </div>
                    <div className="info-field">
                        {error && <p style={{ color: 'red' }}>Error: {error}</p>}
                        {account ? (
                            <div>
                                <p>Email: {account.email}</p>
                                <p>Payment Method: {account.payment_Method}</p>
                                {/* Add other fields you want to display */}
                            </div>
                        ) : (
                            <p>No account found</p>
                        )}
                    </div>
                </div>
            </div>

            {/* Search Section for Genre */}
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
};

export default Account;
