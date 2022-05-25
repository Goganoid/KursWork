import React, { useEffect, useState } from 'react'
import { getUserId } from '../../middleware/storage'
import { getAccountInfo} from '../../middleware/userApi'
import { Link, useParams, useNavigate } from 'react-router-dom'
import { removeTender, unsubscribe } from '../../middleware/tenderApi'
import TenderTable from '../../components/tender-table/TenderTable'
import { getCompanyInfo, getCompanyTendersInfo, removeCompany } from '../../middleware/companyApi'


function Account({ apiClient, setIsLoading, isLoading }) {
    let params = useParams();
    let navigate = useNavigate();
    const [companyId, setCompanyId] = useState();
    const [isOwner, setIsOwner] = useState(false)
    const [userData, setUserData] = useState({});
    const [companyData, setCompanyData] = useState({});
    const [tenders, setTenders] = useState({});
    const [tendersWithParticipation, setTendersWithParticipation] = useState({});
    const [wonTenders, setWonTenders] = useState({});


    const changeCompany = (companyId) => {
        console.log("redirecting");
        navigate(`/account/${companyId}`, { replace: true });
        setCompanyId(companyId);
    }

    useEffect(() => {


        const getData = async () => {

            const curUserId = getUserId();
            if (params.companyId !== undefined) {
                setCompanyId(params.companyId);
                const cResponse = await getCompanyInfo(apiClient, params.companyId);
                const tResponse = await getCompanyTendersInfo(apiClient, params.companyId);
                console.log("FFFFFFF");
                console.log(cResponse);
                console.log(tResponse);
                if (cResponse.status !== 200) window.location.href = `/error/${cResponse.status}`;
                if (tResponse.status !== 200) window.location.href = `/error/${tResponse.status}`;

                const company = cResponse.result;
                const ownsCompany = company.owner.id === curUserId;
                setIsOwner(ownsCompany);
                if (ownsCompany) {
                    const uResponse = await getAccountInfo(apiClient, curUserId);
                    setUserData(uResponse.result);
                } else {
                    setUserData(company.owner);
                }
                let tenders = tResponse.result;
                for (const tenderType in tenders) {
                    tenders[tenderType].map((tender) => {
                        if (tender.pubDate && tender.endDate) {
                            tender.pubDate = new Date(tender.pubDate).toLocaleString('ru-RU');
                            tender.endDate = new Date(tender.endDate).toLocaleString('ru-RU');
                        }
                        return tender
                    });
                }
                console.log(tenders);
                setTenders(tenders.tenders);
                setTendersWithParticipation(tenders.tendersWithParticipation);
                setWonTenders(tenders.wonTendersId);

            } else if (!isNaN(curUserId)) {
                const uResponse = await getAccountInfo(apiClient, curUserId);
                if (uResponse.status !== 200) window.location.href = `/error/${uResponse.status}`;
                const result = uResponse.result;
                setUserData(result);

                if (params.companyId === undefined && result.companies.length !== 0) {
                    changeCompany(result.companies[0].id);
                    return;
                }

                setCompanyData(null);


            } else {
                window.location.href = `/error/404`;
            }
            setIsLoading(false);
        };
        getData();

    }, [params.companyId]);


    const handleDeleteCompanyButton = async (companyId) => {
        
        let activeTenders = tenders.filter(t=>t.propositionsCount>0)
        console.log(activeTenders);
        if(activeTenders.length!==0){
            alert(`У вас є активні тендери з учасниками:\n${activeTenders.map(t=>t.title).join('\n')}`)
        }

        if (window.confirm("Ви справді хочете видалити цю компанію?")) {
            const response = await removeCompany(apiClient, companyId);
            console.log(response);
            if (response.status === 400) alert(response.result.message);
            if (response.status === 200) {
                setUserData({
                    ...userData, companies: userData.companies.filter(company => company.id !== companyId)
                })
                window.location.href = "/account/"
            }
        }
    }


    if (isLoading) return (<div></div>);
    if (companyData == null) {
        return (<div className='container d-flex flex-column align-items-center'>
            <div>Ви ще не створили жодної компанії</div>
            <div>
                <Link to="/company/create">
                    <button className='btn btn-primary'>Створити компанію</button>
                </Link>

            </div>
        </div>)
    }
    return (
        <>
            {isOwner &&
                <div className="d-flex">
                    <select className="form-select mx-1" defaultValue={companyId}
                        onChange={(e) => changeCompany(e.target.value)}>
                        {
                            userData.companies.map((company, idx) => {
                                return (<option key={idx} value={company.id}>{company.name}</option>)
                            })
                        }
                    </select>
                    <button type="button" className="btn btn-danger mx-1" onClick={() => handleDeleteCompanyButton(companyId)}>Видалити</button>
                    <Link to={`/company/edit/${params.companyId}`}>
                    <button type="button" className="btn btn-warning mx-1">Змінити</button>
                    </Link>
                    <Link to={`/company/create`}>
                        <button type="button" className="btn btn-success mx-1">Додати</button>
                    </Link>
                </div>
            }
            <div>
                <div className="fs-1">Дані компанії</div>
                <div>{companyData.name}</div>
                <div>{companyData.location}</div>
                <div>Контактні дані:
                    { getUserId()==userData.id && <Link to={`/account/edit/${userData.id}`}>
                        <button type="button" className="btn btn-warning p-1 mx-2">Змінити</button>
                    </Link>}
                </div>
                <div>{userData.firstName} {userData.lastName}</div>
                <div>Email: {userData.email}</div>

            </div>
            <div className="fs-1">Погоджені тендери</div>
            <TenderTable data={tendersWithParticipation}
                setTendersData={setTendersWithParticipation}
                apiClient={apiClient}
                isOwner={false}
                wonTendersId={wonTenders}
                onDelete={unsubscribe}
            ></TenderTable>
            <div className="d-flex justify-content-between align-items-end">
                {isOwner ? <>
                    <div className="fs-1">Мої тендери</div>
                    <Link to="/tender/create" state={{ companyId }} className="link-primary">Створити тендер</Link>
                </> : <>
                    <div className="fs-1">Тендери компанії</div>
                </>}

            </div>
            <TenderTable
                data={tenders}
                apiClient={apiClient}
                isOwner={isOwner}
                setTendersData={setTenders}
                onDelete={removeTender}></TenderTable>
        </>
    )
}

export default Account