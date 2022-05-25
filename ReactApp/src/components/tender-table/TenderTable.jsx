import React from 'react'
import { Link } from 'react-router-dom'

const tenderStatusStyle = (isWon, isActive, isOwner) => {
    return (isWon ? ['text-success', !isOwner ? 'Вас обрали' : 'Обраний'] : isActive ? ['text-primary', 'У процесі'] : ['text-danger', 'Закінчився']);
}

function TenderTable({
    data,
    setTendersData,
    apiClient,
    isOwner,
    wonTendersId,
    onDelete
}) {

    const tenderRow = (idx, tender, statusColor, statusText, hasRights) => {

        let handleDeleteButton = async (tenderId) => {
            if (window.confirm("Ви справді хочете видалити цей тендер?")) {
                const response = await onDelete(apiClient, tenderId);
                if (response.status === 200) {
                    setTendersData(
                        data.filter(tender => tender.id !== tenderId)
                    )
                }
            }
        }
        return (<tr key={idx}>
            <th scope="row">{idx + 1}</th>
            <td className="col-4"><Link to={`/tender/${tender.id}`}>{tender.title}</Link></td>
            <td className="col-2"><p>{tender.pubDate}</p><p>{tender.endDate}</p></td>
            <td className="col-2">{tender.cost} грн</td>
            <td className={`col-2 ${statusColor}`}>{statusText}</td>
            {hasRights ? <td className="col-2">
                <button type="button" className="btn btn-danger"
                    onClick={() => handleDeleteButton(tender.id)}>Видалити
                </button>
            </td> : null}
        </tr>)

    }
    return (
        <table className="table table-hover">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Назва</th>
                    <th scope="col">Термін</th>
                    <th scope="col">Ціна</th>
                    <th scope="col">Статус</th>
                </tr>
            </thead>
            <tbody>
                {data.map((tender, idx) => {
                    // let isWonTender = wonTendersId != null ?
                    //     (wonTendersId.find(wonTenderId => wonTenderId === tender.id) !== undefined)
                    //     :
                    //     false;
                    let isWonTender = false;
                    if(isOwner){
                        isWonTender = tender.companyExecutorId!=null
                    }
                    else{
                        isWonTender=wonTendersId != null ?
                            (wonTendersId.find(wonTenderId => wonTenderId === tender.id) !== undefined)
                            :
                            false;
                    }
                    
                    let tenderStatus = tenderStatusStyle(isWonTender, tender.isActive, isOwner);
                    return (tenderRow(idx, tender, tenderStatus[0], tenderStatus[1], isOwner));
                })}
            </tbody>
        </table>
    )
}

export default TenderTable