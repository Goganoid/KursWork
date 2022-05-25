import React from 'react'
import {Link} from 'react-router-dom';

function Logout() {
    return (<div className="container">
        <div className="text-center fs-1">Ви вийшли з аккаунту</div>
        <div className="d-flex justify-content-center">
            <Link to='/login'>
                <button type="button" className="btn btn-primary mx-2">Увійти</button>
            </Link>
            <Link to='/'>
                <button type="button" className="btn btn-primary mx-2">На головну</button>
            </Link>
        </div>
    </div>)
}

export default Logout