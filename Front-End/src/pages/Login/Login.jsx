import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import './Login.css';
import logo from '../../assets/logo.png';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5025';

const Login = () => {
    const navigate = useNavigate();
    const [signState, setSignState] = useState("Sign In");
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        apiKey: '',
        paymentMethod: 'ING',
        blocked: false,
        is_Invited: true,
        trial_End_Date: new Date().toISOString().split('T')[0],
    });
    const [user, setUser] = useState(null);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            if (signState === "Sign In") {
                // Existing Sign In logic remains unchanged
                const response = await fetch(
                    `${API_URL}/stored-procedure-get-account-by-email/${encodeURIComponent(formData.email)}`,
                    {
                        method: 'GET',
                        headers: {
                            'Accept': 'application/json',
                            'api-key': formData.apiKey,
                        },
                        credentials: 'include',
                    }
                );

                if (!response.ok) {
                    if (response.status === 401) {
                        const errorContainer = document.getElementById('error-message');
                        if (errorContainer) {
                            errorContainer.textContent = 'Invalid API key';
                            errorContainer.style.display = 'block';
                        }
                        return;
                    }
                    throw new Error('Account not found or server error');
                }

                const user = await response.json();

                if (user && user.email === formData.email) {
                    localStorage.setItem('apiKey', formData.apiKey);
                    setUser(user);
                    navigate('/');
                } else {
                    const errorContainer = document.getElementById('error-message');
                    if (errorContainer) {
                        errorContainer.textContent = "Invalid email or password!";
                        errorContainer.style.display = 'block';
                    }
                }
            } else if (signState === "Sign Up") {
                // Adjusted payload for Sign Up
                const payload = {
                    account_Id: null, // Leave it null, so the backend can handle it
                    email: formData.email,
                    password: formData.password,
                    payment_Method: formData.paymentMethod,
                    blocked: false, // Always false
                    is_Invited: true, // Always true
                    trial_End_Date: new Date().toISOString().split('T')[0], // Current date
                };

                const response = await fetch(`${API_URL}/stored-procedure-create-account`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(payload),
                });

                if (!response.ok) {
                    const errorContainer = document.getElementById('error-message');
                    if (errorContainer) {
                        errorContainer.textContent = "Failed to create account.";
                        errorContainer.style.display = 'block';
                    }
                    throw new Error('Account creation failed');
                }

                const result = await response.json();
                console.log("Account created successfully:", result);
                setSignState("Sign In"); // Redirect user to Sign In
            }
        } catch (error) {
            console.error("Error during submit:", error);
        }
    };



    return (
        <div className="login">
            <img src={logo} className="login-logo" alt="logo" />
            <div className="login-form">
                <h1>{signState}</h1>
                <form onSubmit={handleSubmit}>
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={formData.email}
                        onChange={handleInputChange}
                        required
                    />
                    <input
                        type="password"
                        name="password"
                        placeholder="Password"
                        value={formData.password}
                        onChange={handleInputChange}
                        required
                    />
                    {signState === "Sign In" && (
                        <input
                            type="password"
                            name="apiKey"
                            placeholder="API Key"
                            value={formData.apiKey}
                            onChange={handleInputChange}
                            required
                        />
                    )}
                    {signState === "Sign Up" && (
                        <div className="payment-method">
                            <label>Payment Method</label>
                            <select
                                name="paymentMethod"
                                value={formData.paymentMethod}
                                onChange={handleInputChange}
                                required
                            >
                                <option value="ING">ING</option>
                                <option value="RABOBANK">RABOBANK</option>
                                <option value="coins">Coins</option>
                            </select>
                        </div>
                    )}
                    <button type="submit">{signState}</button>
                    <div id="error-message" style={{ color: 'red', display: 'none' }}></div>
                    <div className="form-help remember">
                        <div className="remember">
                            <input type="checkbox" checked readOnly />
                            <label>Remember Me</label>
                        </div>
                        <p>Need Help?</p>
                    </div>
                </form>
                <div className="form-switch">
                    {signState === "Sign In" ? (
                        <p>
                            New? <span onClick={() => setSignState("Sign Up")}>Sign Up Now</span>
                        </p>
                    ) : (
                        <p>
                            Already have an account? <span onClick={() => setSignState("Sign In")}>Sign In Now</span>
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Login;