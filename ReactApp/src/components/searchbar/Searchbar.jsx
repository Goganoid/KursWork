import React, {useState} from 'react'


function Searchbar(props) {
    const [searchText, setSearchText] = useState("");
    let handleButton = (e) => {
        e.preventDefault();
        props.onSearchTermChange(searchText);
    };

    let onSearchTextChange = (e) => {
        if (searchText.length !== 0 && e.target.value.length === 0) {
            props.onSearchTermChange("");
        }
        setSearchText(e.target.value);
    }
    return (<div className="row mb-4">
        <form className="d-flex flex-row">
            <input className="form-control me-2" type="search" placeholder="Search" aria-label="Search"
                   onChange={onSearchTextChange}/>
            <button className="btn btn-outline-success" type="submit" onClick={handleButton}>Search</button>
        </form>
    </div>)
}

export default Searchbar