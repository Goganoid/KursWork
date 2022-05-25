import React from 'react'
import {Link} from 'react-router-dom'
import LoginControl from '../login-control/LoginControl'

function Navbar() {
    return (
        <nav className="navbar navbar-expand-lg navbar-light border-bottom border-2 mb-3">
            <div className="container-fluid ">
                <Link to="/" className="navbar-brand">Trading</Link>
                <button className="navbar-toggler my-2" type="button" data-bs-toggle="collapse"
                        data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"/>
                </button>
                <div className="collapse navbar-collapse flex-grow-0" id="navbarSupportedContent">
                    <ul className="navbar-nav align-items-end ">
                        <LoginControl/>
                    </ul>
                </div>
            </div>
        </nav>
    )
}

export default Navbar