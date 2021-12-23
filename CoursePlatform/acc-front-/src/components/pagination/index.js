import PagePagination from './pagination';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { changeCurrentPage } from '../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        totalElementsCount: stateRedux.panelReducer.totalElementsCount,
        currentPage: stateRedux.panelReducer.currentPage
    }
}
const mapDispatchToProps = {
    changeCurrentPage
}

export default withRouter(connect(mapState, mapDispatchToProps)(PagePagination));