import NavBar from '../../components/Navbar/Navbar.jsx';
import React, { useState, useEffect } from "react";
import TitleCard from "../../components/TitleCards/TitleCard.jsx";
import './Account.css';

const Account = () => {
    const [email, setEmail] = useState("");         // Store the email entered by the user
    const [account, setAccount] = useState(null);   // Store account data returned by the API
    const [error, setError] = useState(null);       // Store any errors from the API
    const [searchQuery, setSearchQuery] = useState('');  // For search by genre
    const [searchEmail, setSearchEmail] = useState(''); // For account search by email
    const [allAccounts, setAllAccounts] = useState([]); // Store all accounts from API

    // Fetch account data based on email
    useEffect(() => {
        if (email) {
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

    // Fetch all accounts
    useEffect(() => {
        const fetchAllAccounts = async () => {
            try {
                const response = await fetch(`http://localhost:5025/api/account`);
                if (!response.ok) {
                    throw new Error("Failed to fetch accounts");
                }
                const data = await response.json();
                setAllAccounts(data);
            } catch (err) {
                console.error(err);
            }
        };

        fetchAllAccounts();
    }, []);

    const handleSearchChange = (e) => {
        setSearchQuery(e.target.value);
    };

    const handleSearch = () => {
        console.log('Search for:', searchQuery);
    };

    const handleSearchEmailChange = (e) => {
        setSearchEmail(e.target.value);
    };

    const handleEmailSearch = () => {
        setEmail(searchEmail); // Trigger useEffect to fetch account
    };

    return (
        <div className="account-page">
            <NavBar />
            {/* Account Information Section */}
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
                        <p>{account?.trialEndDate || 'N/A'}</p>
                    </div>
                </div>
            </div>

            {/* Movies by Genre Section */}
            <div className="search-section">
                <h2>Search Movies by Genre</h2>
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

            {/* Search Account by Email Section */}
            <div className="search-section email-search-section">
                <h2>Search Account by Email</h2>
                <div className="search-container">
                    <input
                        type="email"
                        value={searchEmail}
                        onChange={handleSearchEmailChange}
                        placeholder="Enter email to search"
                        className="search-input"
                    />
                    <button onClick={handleEmailSearch} className="search-button">
                        Search
                    </button>
                </div>
                {error && <p style={{color: 'red'}}>Error: {error}</p>}
            </div>

            {/* Display All Accounts Section */}
            <div className="all-accounts-section">
                <h2>All Accounts</h2>
                {allAccounts.length > 0 ? (
                    <div className="scroll-container">
                        {allAccounts.map((acc) => (
                            <div className="account-card" key={acc.id}>
                                <p><strong>Email:</strong> {acc.email}</p>
                                <p><strong>Payment Method:</strong> {acc.payment_Method}</p>
                                <p><strong>Trial End Date:</strong> {acc.trialEndDate}</p>
                            </div>
                        ))}
                    </div>
                ) : (
                    <p>Loading accounts or no accounts available...</p>
                )}
            </div>
        </div>
    );
};

export default Account;
