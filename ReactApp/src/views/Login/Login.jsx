import React, {useState} from 'react'
import {Link} from 'react-router-dom'
import {login} from '../../middleware/authenticateApi';
import {setUserData} from '../../middleware/storage';

function Login({apiClient}) {

    let [email, setEmail] = useState("");
    let [password, setPassword] = useState("");
    let [invalidLogin, setInvalidLogin] = useState(false);
    let handleSumbit = async e => {
        e.preventDefault();
        let response = await login(apiClient, email, password);
        console.log(response);
        if (response.status === 401) {
            setInvalidLogin(true);
        } else {
            setInvalidLogin(false);
            setUserData({token: response.result.token, userId: response.result.userId});
            window.location.href = "/catalog";
        }
    }
    return (<div className="container w-25 mt-5">
        <form onSubmit={handleSumbit}>
            <div className="mb-3">
                <label htmlFor="InputEmail" className="form-label">Пошта</label>
                <input type="email" className="form-control" id="InputEmail"
                       onChange={e => setEmail(e.target.value)}/>
            </div>
            <div className="mb-3">
                <label htmlFor="InputPassword" className="form-label">Пароль</label>
                <input type="password" className="form-control" id="InputPassword"
                       onChange={e => setPassword(e.target.value)}/>
            </div>
            <button type="submit" className="btn btn-primary">Увійти</button>
            <div className="mt-3">
                <Link to="/register">Не маєте акаунту?</Link>
            </div>
        </form>
        {invalidLogin && <div className="text-danger">Неправильний пароль чи email!</div>}
    </div>)
}

export default Login