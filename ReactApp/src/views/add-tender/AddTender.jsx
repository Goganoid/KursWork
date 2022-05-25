import React, {useState} from 'react'
import {useLocation} from 'react-router-dom'
import {postTender} from '../../middleware/tenderApi';

const validationError = (value) => {
    return (<div className="text-danger">{value}</div>)
}



function AddTender({apiClient}) {
    const [errorMessage, setErrorMessage] = useState("");
    const [submitted, setSubmitted] = useState(false);
    const [title, setTitle] = useState("");
    const [endDate, setEndDate] = useState(null);
    const [cost, setCost] = useState("");
    let location = useLocation();
    const companyId = location.state.companyId;
    let handleSumbit = async e => {
        e.preventDefault();
        setSubmitted(true);
        if (title.trim().length !== 0 && endDate !== '' && cost !== '') {
            let response = await postTender(apiClient, companyId, endDate, title, cost);
            console.log(response);
            if (response.status !== 200) {
                let error = "";
                if(response.result.errors!=null){
                    for(let field in response.result.errors){
                        error+=`${response.result.errors[field][0]}\n`;
                    }
                }
                setErrorMessage(error.length===0 ? "Сталася помилка":error);
            } else {

                window.location.href = `/account/${companyId}`;
            }
        }
    }


    const notEmpty = "Поле не повинно бути пустим"
    return (<>
        <div className="container w-50 mt-5">
            <form onSubmit={handleSumbit}>
                <div className="mb-3">
                    <label forHtml="InputTitle" className="form-label">Назва</label>
                    <input type="text" className="form-control" id="InputTitle"
                           onChange={e => setTitle(e.target.value)}/>
                    {submitted && title.trim().length === 0 && validationError(notEmpty)}
                </div>
                <div className="mb-3">
                    <label forHtml="InputEndDate" className="form-label">Кінцевий термін</label>
                    <input type="datetime-local" className="form-control" id="InputEndDate"
                           min={new Date().toISOString().slice(0, -8)} onChange={e => setEndDate(e.target.value)}/>
                    {submitted && endDate == null && validationError(notEmpty)}
                </div>
                <div className="mb-3">
                    <label forHtml="InputCost" className="form-label">Ціна</label>
                    <input type="number" className="form-control" id="InputCost" min="3000"
                           onChange={e => setCost(e.target.value)}/>
                    {submitted && cost.length === 0 && validationError(notEmpty)}
                </div>

                <button type="submit" className="btn btn-primary">Створити тендер</button>
            </form>
            {errorMessage !== "" && <div className="text-danger">{errorMessage}</div>}
        </div>

    </>)
}

export default AddTender