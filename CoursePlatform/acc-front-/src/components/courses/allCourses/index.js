import AllCourses from './allCourses';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert,
    resetIsSortChangedStatus,
    setTotalCount,
    resetIsElementsChanged,
    clearTotalCount
} from '../../../actions/general';

const mapState = (stateRedux) => {
    return {
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        sortDirection: stateRedux.panelReducer.sortDirection,
        sortBy: stateRedux.panelReducer.sortBy,
        currentPage: stateRedux.panelReducer.currentPage,

        isSortingChanged: stateRedux.panelReducer.isSortingChanged,
        isElementsChanged: stateRedux.panelReducer.isElementsChanged
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    resetIsSortChangedStatus,
    setTotalCount,
    resetIsElementsChanged,
    clearTotalCount
}

export default withRouter(connect(mapState, mapDispatchToProps)(AllCourses));