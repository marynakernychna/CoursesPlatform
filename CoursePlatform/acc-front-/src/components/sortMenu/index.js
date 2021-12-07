import SortMenu from './sortMenu';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert,
    openModal,
    changeElementsOnPageCount,
    changeElementsSortDirection,
    changeElementsSortBy
} from '../../actions/general';

const mapState = (stateRedux) => {
    return {
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        sortDirection: stateRedux.panelReducer.sortDirection,
        sortBy: stateRedux.panelReducer.sortBy,
        totalElementsCount: stateRedux.panelReducer.totalElementsCount
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    openModal,
    changeElementsOnPageCount,
    changeElementsSortDirection,
    changeElementsSortBy
}

export default withRouter(connect(mapState, mapDispatchToProps)(SortMenu));