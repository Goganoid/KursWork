import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { getTender, setExecutor, subscribeToTender, toggleTender, unsubscribe } from '../../middleware/tenderApi';
import { Link } from 'react-router-dom';
import './tender.css'
import { getUserId, isLoggedIn } from '../../middleware/storage';
import getRemainingTime from '../../utils/getRemainingTime';
import { getCompaniesList } from '../../middleware/userApi';


function Timer({ endDate, tender, hasTime, setHasTime }) {
    let [timerInterval, setTimerInterval] = useState(null);
    const [remainingTime, setRemainingTime] = useState(null);
    useEffect(() => {
        setTimerInterval(setInterval(() => {
            if (endDate != null) {
                setRemainingTime(getRemainingTime(endDate));
            }
            return () => {
                clearInterval(timerInterval);
            }
        }, 1000))
        return ()=>{
            clearInterval(timerInterval);
        }
    }, [endDate])
    if (!hasTime) {
        clearInterval(timerInterval);
        return (<><p>Час скінчився!</p></>)
    }
    if (remainingTime == null) return (<></>);
    setHasTime((remainingTime.days !== 0 || remainingTime.hours !== 0 || remainingTime.minutes !== 0 || remainingTime.seconds !== 0))
    return (<>
        <p>Час: {(remainingTime.days !== 0 ? remainingTime.days + "д " : "") + remainingTime.hours + "год " + remainingTime.minutes + "хв " + remainingTime.seconds + "с"}</p>
        <p style={{ 'fontSize': '10px' }}>{tender.pubDate} - {tender.endDate} </p>
    </>);
}


function Tender({ apiClient, setIsLoading, isLoading }) {
    let params = useParams();
    const userId = getUserId();
    let [tender, setTender] = useState({});
    const [userCompanies, setUserCompanies] = useState(null);
    const [hasTime, setHasTime] = useState(true);
    const [cost, setCost] = useState(0);
    const [companySubscriber, setCompanySubscriber] = useState(null);
    useEffect(() => {
        setIsLoading(true);

        Promise.all([getCompaniesList(apiClient, userId), getTender(apiClient, params.tenderId)]).then((responses) => {
            let [cResponse, tResponse] = responses;
            console.log("FFFFFF");
            console.log(cResponse);
            console.log(tResponse);
            if (cResponse !== undefined && cResponse.status === 200) {
                console.log(cResponse);
                setUserCompanies(cResponse.result)
                if (cResponse.result.length !== 0) {
                    setCompanySubscriber(cResponse.result[0].id);
                }
            }
            if (tResponse.status !== 200) window.location.href = "/error/" + tResponse.status;
            let tender = tResponse.result;
            // tender.propositions = tResponse.result.propositions;
            tender.pubDate = new Date(tender.pubDate).toLocaleString('ru-RU');
            let tEndDate = new Date(tender.endDate).getTime();
            if(tEndDate<new Date().getTime()) setHasTime(false);
            tender.endDate = new Date(tender.endDate).toLocaleString('ru-RU');
            tender.milisEndDate = tEndDate;
            setTender(tender);
            setIsLoading(false);
        });
        return () => {
        };
    }, []);


    if (isLoading) return (<></>);
    let isOwner = tender.companyOrganizer.userOwnerId === userId;
    let isSubscribed = tender.propositions.find(p => p.userId === userId) !== undefined;

    let handleToggleTender = async () => {
        const response = await toggleTender(apiClient, tender.id);
        if (response.status === 200) {
            setTender({ ...tender, isActive: response.result.isActive });
        }
    }

    let handleSubscribeToTender = async () => {
        if (isSubscribed){
            const response = await unsubscribe(apiClient,tender.id);
            if(response.status===200){
                setTender({...tender,propositions:tender.propositions.filter(p=>p.userId!==userId)});
            }
            isSubscribed=false;
        }
        else{
            console.log(`cost:${cost}`)
            if (cost < 0) {
                alert("Неправильно вказана ціна!");
                return;
            }
            const response = await subscribeToTender(apiClient, companySubscriber, tender.id, cost);
            console.log(response);
            if (response.status === 200) {
                setTender({ ...tender, propositions: response.result });
            }
        }
    }
    let handleSelectExecutor = async (executorId) => {
        const response = await setExecutor(apiClient, tender.id, executorId);
        if (response.status === 200) {
            await setTender({ ...tender,
                 companyExecutorId: executorId, 
                 isActive: false,
                 milisEndDate:new Date().getTime() });
            await setHasTime(false);
        }
    }

    let timer = () => {
        return hasTime ?
            <Timer endDate={tender.milisEndDate} hasTime={hasTime} setHasTime={setHasTime} tender={tender}></Timer>
            : <p>{tender.executorId != null ? "Виконавця обрано!" : "Час скінчився!"}</p>
    }

    let tenderControls = () => {
        if (!hasTime) return;
        if (isOwner) {
            return (<button
                type="button"
                className={`btn ${tender.isActive ? 'btn-danger' : 'btn-success'} ${hasTime ? 'active' : 'disabled'} mb-3 px-2 w-100`}
                onClick={handleToggleTender}>
                {tender.isActive ? 'Зупинити тендер' : 'Відновити тендер'}
            </button>);
        } else if (userCompanies != null) {
            return (<>
                {
                    userCompanies.length !== 0
                        ?
                        <button
                            type="button"
                            className={`btn ${isSubscribed ? 'btn-danger' : 'btn-primary'} mb-3 px-2 w-100 ${tender.isActive && hasTime ? 'active' : 'disabled'} `}
                            onClick={handleSubscribeToTender}>
                            {isSubscribed ? 'Відкликати пропозицію' : 'Подати пропозицію'}
                        </button>
                        : <button className='btn btn-primary disabled'>Створіть компанію, щоб подати пропозицію</button>
                }

                {isSubscribed && userCompanies.length !== 0
                    ? null
                    : <>
                        <label forHtml="InputCost" className="form-label">Ціна</label>
                        <input type="number" className="form-control" id="InputCost"  min="3000"
                            onChange={e => setCost(e.target.value)} />
                        <select className="form-select" onChange={(e) => setCompanySubscriber(e.target.value)}>
                            {
                                userCompanies.map((company, idx) => {
                                    return (<option key={idx} value={company.id}>{company.name}</option>)
                                })
                            }
                        </select>
                    </>
                }
            </>);
        } else if (!isLoggedIn()) {
            return (<Link to='/login'>
                <button className='btn btn-primary mb-3 px-2 w-100'>Увійти</button>
            </Link>)
        }
    }
    return (<>
        <div className="w-100 mt-5 px-5 ">
            <div className="tender row">
                <div className="tender-about col-8">
                    <div className="tender-info">
                        <div className="mx-1">Тендер {tender.id}</div>
                        <div className="mx-1"> Опубліковано: {tender.pubDate}</div>
                    </div>
                    <div className="tender-title">
                        {tender.title}
                    </div>
                    <div className="tender-info-item d-flex">
                        <div className="tender-info-item-left">Організатор: <Link
                            to={`/account/${tender.companyOrganizer.id}`}>{tender.companyOrganizer.name}</Link></div>
                    </div>
                    <div className='tender-info-item '>
                        {tender.companyOrganizer.location}
                    </div>
                    <div className='tender-info-item '>
                        Ціна:{tender.cost}
                    </div>
                </div>
                <div className="tender-status col-4">
                    <div className="bg-success text-wrap text-center py-2 my-1 mt-3">Прийом пропозицій:</div>
                    <div className="bg-warning tender-status my-1">
                        {timer()}
                    </div>
                    {tenderControls()}
                </div>
            </div>
            <div className="tender-control row">
                <div className="fs-1">Пропозиції</div>
                <table className="table table-hover">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Виконавець</th>
                            <th scope="col">Ціна</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tender.propositions.map((proposition, idx) => {
                            console.log(`companyExecutor ${tender.companyExecutorId} proposition ${proposition.companyId}`);
                            return (<tr>
                                <th scope="row">{idx + 1}</th>
                                <td><Link to={`/account/${proposition.companyId}`}
                                    className="link-primary">{proposition.companyName}</Link>
                                </td>
                                <td>
                                    {proposition.cost} грн
                                </td>
                                <td>
                                    {isOwner 
                                    ? (tender.companyExecutorId === proposition.companyId 
                                        ? <button type="button"
                                        className='btn btn-success disabled w-100'>Обраний</button> 
                                        : <button type="button"
                                            className={`btn btn-primary w-100 ${(hasTime || tender.companyExecutorId == null) ? 'active' : 'disabled'}`}
                                            onClick={() => handleSelectExecutor(proposition.companyId)}>Обрати</button>) 
                                    : (tender.companyExecutorId === proposition.companyId) && (<p className="text-success">Обраний</p>)
                                    }
                                </td>

                            </tr>);
                        })}

                    </tbody>
                </table>
            </div>
        </div>
    </>)
}

export default Tender