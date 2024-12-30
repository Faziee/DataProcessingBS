import React from "react";
import './TitleCard.css'
import cards_data from '../../assets/cards/Cards_data.js'

const TitleCard = ({title, category}) => {
    return(
        <div className='title-card'>
            <h2>{title ? title: "Popular on Netflix"}</h2>
            <div className='cards-list'>
                {cards_data.map((card, index) =>
                {
                    return <div className='card' key={index}>
                        <img src={card.image} alt={card.name}/>
                        <p>{card.name}</p>
                    </div>
                })}
            </div>
        </div>
    )
}

export default TitleCard;