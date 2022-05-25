import React, { useEffect } from 'react'
import TenderCard from '../tender-card/TenderCard'
import { useState, useMemo } from 'react'
import Pagination from '../pagination/Pagination';
import { getTenders } from '../../middleware/tenderApi';

function createCards(tenders) {
    let cards = [];
    for (let i = 0; i < tenders.length; i++) {
        cards.push(<TenderCard key={i}{...tenders[i]} />)
    }
    return cards;
}

function CardsList({ titleFilter, PageSize, apiClient }) {
    let [cards, setCards] = useState([]);
    let [loading, setLoading] = useState(true);
    useEffect(() => {
        setLoading(true);
        getTenders(apiClient, 50, titleFilter).then((response) => {
            console.log("tenders");
            console.log(response);
            let tenders = response.result;
            tenders.map((tender) => {
                tender.pubDate = new Date(tender.pubDate);
                tender.endDate = new Date(tender.endDate);
                return tender;
            })
            setCards(createCards(tenders));
            setLoading(false);
        })
    }, [titleFilter,apiClient]);
    const [currentPage, setCurrentPage] = useState(1);
    const currentPageData = useMemo(() => {
        const firstPageIndex = (currentPage - 1) * PageSize;
        const lastPageIndex = firstPageIndex + PageSize;
        return cards.slice(firstPageIndex, lastPageIndex);
    }, [currentPage, cards,PageSize]);

    return (<>
        <div>
            {loading ? <div>Loading...</div> : cards.length !== 0 ? currentPageData : "Нічого не знайдено"}
        </div>
        <Pagination
            className="pagination-bar"
            currentPage={currentPage}
            totalCount={cards.length}
            pageSize={PageSize}
            onPageChange={page => setCurrentPage(page)}
        />
    </>)
}

export default CardsList