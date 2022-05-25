import React, {useState} from 'react'
import './loading.css';

function Loading({children}) {
    const [isLoading, setIsLoading] = useState(true);
    return (<>
            {React.cloneElement((children), {...children.props, setIsLoading, isLoading})}
            {isLoading ? <div className="loading">
                <span className="fs-1">Loading...</span>
                <div className="lds-ellipsis">
                    <div></div>
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
            </div> : null}
        </>

    )
}

export default Loading