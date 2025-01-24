import React, {useEffect, useRef} from "react";
import './Navbar.css'
import logo from '../../assets/logo.png'
import search_icon from '../../assets/search_icon.svg'
import bell_icon from '../../assets/bell_icon.svg'
import profile_img from '../../assets/profile_img.png'
import dropdown_icon from '../../assets/caret_icon.svg'
import { Link } from 'react-router-dom';




const Navbar = () => {

    const navRef = useRef();

    useEffect(() => {
        window.addEventListener('scroll', ()=>{
            if(window.scrollY >= 80){
                navRef.current.classList.add('nav-dark')
            } else{
                navRef.current.classList.remove('nav-dark')
            }
        })
    }, []);

    return (
        <div ref={navRef} className='navbar'>
            <div className='navbar-left'>
                <img src={logo} alt="Netflix logo"/>
                <ul>
                    <li> <Link to="/" className="no-decoration">Home</Link></li>
                    <li>Tv Shows</li>
                    <li>Movies</li>
                    <li>New and Popular</li>
                    <li>My List</li>
                    <li>Browse by language</li>
                </ul>
            </div>
            <div className='navbar-right'>
                <img src={search_icon} alt='search icon' className='icons'/>
                <p>Name here of user</p>
                <img src={bell_icon} alt='bell icon' className='icons'/>
                <div className='navbar-profile'>
                    <img src={profile_img} alt='bell icon' className='profile'/>
                    <img src={dropdown_icon} alt='bell icon' />
                    <div className='dropdown'>
                        <p><Link to="/account" className='no-decoration'>Account</Link></p>
                        <p style={{textDecoration: 'underline'}}>Sign out</p>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Navbar;