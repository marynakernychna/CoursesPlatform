import React from 'react';
import { Pagination } from 'antd';

class PagePagination extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            currentPage: this.props.currentPage,
            totalElementsCount: this.props.totalElementsCount,
            elementsOnPage: this.props.elementsOnPage
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            currentPage: nextProps.currentPage,
            elementsOnPage: nextProps.elementsOnPage,
            totalElementsCount: nextProps.totalElementsCount
        }
    }

    showPagination = () => {
        return <div style={{ textAlign: 'center' }}>
            <Pagination
                current={this.state.currentPage}
                pageSize={this.state.elementsOnPage}
                total={this.state.totalElementsCount}
                showTotal={(total, range) => `${range[0]}-${range[1]} of ${total} items`}
                onChange={this.changeCurrentPage}
                style={{ paddingTop: '20px' }}
            />
        </div>
    }

    changeCurrentPage = (page) => {

        const {
            changeCurrentPage
        } = this.props;

        changeCurrentPage(page);
    };

    render() {

        return (
            <>
                {this.showPagination()}
            </>
        );
    }
}

export default PagePagination;