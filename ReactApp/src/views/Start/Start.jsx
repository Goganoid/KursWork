import React from 'react'
import { Link } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';

function Start() {
    return (<>
        <div className="bg-primary d-flex flex-column justify-content-center align-items-center"
            style={{ height: "100vh" }}>
            <div className="fs-1 text-light text-uppercase">Сайт тендерів</div>
            <Link to="/catalog">
                <button type="button" className="btn btn-success m-3 px-5">Подивитися пропозиції</button>
            </Link>
            <div className="d-flex">
                <Link to="/login">
                    <button type="button" className="btn btn-link m-3 px-5 text-light">Увійти</button>
                </Link>
                <Link to="/register">
                    <button type="button" className="btn btn-link m-3 px-5 text-light">Зареєструватися</button>
                </Link>
            </div>

        </div>
    </>)
}

export default Start