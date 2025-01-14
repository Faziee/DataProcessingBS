import React from "react";
import './Home.css'
import NavBar from '../../components/Navbar/Navbar.jsx'
import hero_banner from '../../assets/hero_banner.jpg'
import hero_title from '../../assets/hero_title.png'
import play_icon from '../../assets/play_icon.png'
import info_icon from '../../assets/info_icon.png'
import TitleCard from "../../components/TitleCards/TitleCard.jsx";
import Footer from "../../components/Footer/Footer.jsx";

const Home = () => {
    return (
        <div className='home'>
            <NavBar/>
            <div className='hero_banner'>
                <img src={hero_banner} alt='banner' className='banner-image'/>
                <div className='hero-caption'>
                    <img src={hero_title} alt='title for the banner' className='caption-img'/>
                    <p>
                        Discovering his ties to a secret ancient order, a young man living in
                        modern Istanbul embarks on a quest to save the city from an immortal enemy.
                    </p>
                    <div className='banner-btn-container'>
                        <button className='btn-banner'>
                            <img src={play_icon} alt='play button'/>
                            Play
                        </button>
                        <button className='btn-banner dark-btn'>
                            <img src={info_icon} alt='infomation button'/>
                             More Information
                        </button>
                    </div>
                    <TitleCard/>
                </div>
            </div>
            <div className='more-cards'>
                <TitleCard title={'Blockbuster movies'} category={"top_rated"}/>
                <TitleCard title={'Only on Netflix'} category={"popular"}/>
                <TitleCard title={'Upcoming'} category={"upcoming"}/>
                <TitleCard title={'Top Pics for you'} category={"now_playing"}/>
            </div>
            <Footer/>
        </div>
    )
}

export default Home;