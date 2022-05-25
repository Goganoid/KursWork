import React, {useState} from 'react'
import {addCompany, editCompany} from '../../middleware/companyApi';
import { useParams } from 'react-router-dom';
const validationError = (value) => {
    return (<div className="text-danger">{value}</div>)
}



function AddCompany({apiClient}) {
    const [errorMessage, setErrorMessage] = useState("");
    const [submitted, setSubmitted] = useState(false);
    const [name, setName] = useState("");
    const [location, setLocation] = useState("");

    const params = useParams();
    let editId = params.companyId;
    // alert(editId);
    let handleSumbit = async e => {
        e.preventDefault();
        setSubmitted(true);
        if (name.length !== '' && location !== '') {
            let response = editId==null 
            ? await addCompany(apiClient, name, location)
            : await editCompany(apiClient,editId,name,location);
            console.log(response);
            if (response.status !== 200) {
                setErrorMessage(response.result?.message ?? "Сталася помилка");
            } else {
                window.location.href = `/account/${response.result.id}`;
            }
        }
    }
    const notEmpty = "Поле не повинно бути пустим";
    return (<>
        <div className="container w-50 mt-5">
            <form onSubmit={handleSumbit}>
                <div className="mb-3">
                    <label forHtml="InputTitle" className="form-label">Назва</label>
                    <input type="text" className="form-control" id="InputTitle"
                           onChange={e => setName(e.target.value)}/>
                    {submitted && name.length === 0 && validationError(notEmpty)}
                </div>
                <div className="mb-3">
                    <label forHtml="InputLocation" className="form-label">Місце розташування</label>
                    <input type="text" className="form-control" id="InputLocation"
                           onChange={e => setLocation(e.target.value)}/>
                    {submitted && location.length === 0 && validationError(notEmpty)}
                </div>
                <button type="submit" className="btn btn-primary">{editId==null?'Створити компанію':'Редагувати'}</button>
            </form>
            {errorMessage !== "" && <div className="text-danger">{errorMessage}</div>}
        </div>

    </>)
}

export default AddCompany