import React from 'react'
import {usePagination, DOTS} from './usePagination';
import classnames from 'classnames';

const Pagination = props => {
    const {
        onPageChange, totalCount, siblingCount = 1, currentPage, pageSize
    } = props;
    const paginationRange = usePagination({
        currentPage, totalCount, siblingCount, pageSize
    });
    if (currentPage === 0 || paginationRange.length < 2) {
        return null;
    }

    const onNext = () => {
        onPageChange(currentPage + 1);
    };

    const onPrevious = () => {
        onPageChange(currentPage - 1);
    };

    let lastPage = paginationRange[paginationRange.length - 1];
    return (

        <>
            <nav>
                <ul className="pagination">
                    <li className={classnames('page-item', {disabled: currentPage === 1})} onClick={onPrevious} key="0">
                        <span className="page-link">Previous</span></li>
                    {paginationRange.map((pageNumber, idx) => {

                        // If the pageItem is a DOT, render the DOTS unicode character
                        if (pageNumber === DOTS) {
                            return <li className="page-item disabled"><span className="page-link">&#8230;</span></li>;
                        }

                        // Render our Page Pills
                        return (<li
                            className={classnames('page-item', {
                                active: pageNumber === currentPage
                            })}
                            onClick={() => onPageChange(pageNumber)}
                            key={idx + 1}
                        >
                            <span className="page-link">{pageNumber}</span>
                        </li>);
                    })}

                    <li className={classnames('page-item', {disabled: currentPage === lastPage})} onClick={onNext}
                        key={paginationRange.length + 1}><span className="page-link">Next</span></li>
                </ul>
            </nav>
        </>)
}

export default Pagination