import React from 'react'
import { useState } from 'react'
import Searchbar from '../../components/searchbar/Searchbar'
import CardsList from '../../components/cards-list/CardsList'

function Catalog(props) {
    const [searchTerm, setSearchTerm] = useState("");
    return (
        <>
            <Searchbar onSearchTermChange={setSearchTerm} />
            <div className="main m-1">
                <CardsList titleFilter={searchTerm} PageSize={5} apiClient={props.apiClient} />
            </div>

        </>
    )
}

export default Catalog