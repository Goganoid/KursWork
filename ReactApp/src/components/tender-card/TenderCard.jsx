import React from 'react'
import { Link } from 'react-router-dom';
import getRemainingTime from '../../utils/getRemainingTime';


function TenderCard(props) {
    let { days, hours, minutes } = getRemainingTime(new Date(props.endDate).getTime());
    let timeFormatted = "";
    if (days > 0) {
        timeFormatted += `${days} дн. `;
    }
    if (hours >= 0) {
        timeFormatted += `${hours} год. `;
    }
    if (days <= 0) {
        timeFormatted += `${minutes} хв.`;
    }

    return (<div className="m-2 p-2 row trade-card" style={{ backgroundColor: "#fff" }}>
        <div className="naming p-2 col d-flex flex-column border-2 border-end">
            <Link to={`/tender/${props.id}`} className="trade-subject link-primary mb-2 mt-3">
                {props.title}
            </Link>
            <Link to={`/account/${props.companyOrganizerId}`}
                className="link-primary trade-organizer mb-2">{props.companyOrganizer.name}</Link>
            <div className="trade-number">
                Опубліковано: <span className="fw-bold">{props.pubDate.toLocaleString('ru-RU')}</span>
            </div>
        </div>
        <div className="naming p-2 col-3  border-2 border-end d-flex flex-column align-items-center ">
            <div className="bg-primary text-wrap badge py-2 my-1 mt-3">Прийом пропозицій до:</div>
            <div className="time py-1">{props.endDate.toLocaleString('ru-RU')}</div>
            <div className="time py-3">Залишилося <br /> <span className="fw-bold">{timeFormatted}</span></div>
        </div>
        <div className="naming p-2  col d-flex flex-column align-items-center justify-content-between ">
            <div className="fw-bold mt-3">{props.cost} грн</div>
            <Link to={`/tender/${props.id}`}>
                <button type="button" className="btn btn-primary mb-3 px-5">Переглянути</button>
            </Link>
        </div>
    </div>)
}

export default TenderCard