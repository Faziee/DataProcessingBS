import React from 'react'
import Home from './pages/Home/Home.jsx'
import {Routes, Route} from "react-router-dom";
import Login from "./pages/Login/Login.jsx";
import Player from "./pages/Player/Player.jsx"
import Account from "./pages/Account/Account.jsx";

const App = () => {
    return (
        <div>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path='/player/:id' element={<Player/>} />
                <Route path='/account' element={<Account />} />
            </Routes>
        </div>
    )
}

export default App