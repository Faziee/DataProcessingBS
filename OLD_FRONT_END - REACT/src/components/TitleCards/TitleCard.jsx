import React, { useEffect, useRef, useState } from "react";
import './TitleCard.css';
import {Link} from "react-router-dom";

const TitleCard = ({ title,  category}) => {
    const [apiData, setApiData] = useState([]);
    const cardRef = useRef();

    const options = {
        method: 'GET',
        headers: {
            accept: 'application/json',
            Authorization: `Bearer ${import.meta.env.VITE_TMDB_API_KEY}`
        }
    };

    const handleWheel = (event) => {
        event.preventDefault();
        cardRef.current.scrollLeft += event.deltaY;
    };

    useEffect(() => {
        fetch(`https://api.themoviedb.org/3/movie/${category ? category : "now_playing"}?language=en-US&page=1`, options)
            .then(res => res.json())
            .then(res => setApiData(res.results || []))
            .catch(err => console.error(err));
    }, []);

    return (
        <div className='title-card'>
            <h2>{title || "Popular on Netflix"}</h2>
            <div className='cards-list' ref={cardRef} onWheel={handleWheel}>
                {apiData.map((card, index) => (
                    <Link to={`/Player/${card.id}`} className='card' key={index}>
                        <img src={`https://image.tmdb.org/t/p/w500${card.backdrop_path}`} alt={card.original_title || "No Title"} />
                        <p>{card.original_title || "No Title Available"}</p>
                    </Link>
                ))}
            </div>
        </div>
    );
};

export default TitleCard;
