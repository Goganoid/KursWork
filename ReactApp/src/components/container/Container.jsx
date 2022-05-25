import React from 'react'
import Navbar from '../navbar/Navbar';
import {Outlet} from 'react-router-dom';

function Container() {
    return (<div className="App container w-75">
        <Navbar></Navbar>
        <Outlet></Outlet>
    </div>)
}

export default Container