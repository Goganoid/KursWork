import React, {useState} from 'react'
import {isLoggedIn} from '../../middleware/storage'
import {Link} from 'react-router-dom'

function LoginControl() {

    let [loggedIn, setLoggedIn] = useState(isLoggedIn());

    let handleLogoutClick = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userId');
        setLoggedIn(isLoggedIn);
    }
    let loginMenu;
    if (loggedIn) {
        loginMenu = <>
            <li className="nav-item">
                <Link to={`/account/`}>
                    <button type="button" className="btn btn-primary nav-link mx-2">Кабінет</button>
                </Link>
            </li>
            <li className="nav-item">
                <Link to='/logout'>
                    <button type="button" className="btn btn-link nav-link mx-2" onClick={handleLogoutClick}>Вийти
                    </button>
                </Link>
            </li>
        </>;
    } else {
        loginMenu = <>
            <li className="nav-item">
                <Link to='/login'>
                    <button type="button" className="btn btn-primary nav-link mx-2">Увійти</button>
                </Link>
            </li>
            <li className="nav-item">
                <Link to='/register'>
                    <button type="button" className="btn btn-outline-success nav-link ">Зареєструватися</button>
                </Link>
            </li>
        </>
    }
    return (loginMenu);

}

export default LoginControl