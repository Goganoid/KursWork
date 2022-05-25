import React from 'react'
import './errorpage.css'
import {Link, useParams} from 'react-router-dom'

const errors = new Map()
    .set('404', "404 Not Found")

function ErrorPage() {
    let params = useParams()
    return (<div className="container">
            <div className="row">
                <div className="col-md-12">
                    <div className="error-template">
                        <h1>Oops!</h1>
                        <h2>{errors.get(params.errorCode)}</h2>
                        <div className="error-details">
                            Sorry, an error has occured
                        </div>
                        <div className="error-actions">
                            <Link to="/" className="btn btn-primary btn-lg">Take Me Home</Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    )
}

export default ErrorPage