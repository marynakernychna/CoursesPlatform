import CoursesPage from './coursesPage';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert,
    openModal,
    resetIsSortChangedStatus,
    changeCurrentPage
} from '../../actions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        isAlert: stateRedux.generalReducer.isAlert,
        isModal: stateRedux.generalReducer.isModal,

        sectionName: stateRedux.panelReducer.sectionName,
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        totalElementsCount: stateRedux.panelReducer.totalElementsCount,
        currentPage: stateRedux.panelReducer.currentPage,

        isElementsChanged: stateRedux.panelReducer.isElementsChanged
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    openModal,
    resetIsSortChangedStatus,
    changeCurrentPage
}

export default withRouter(connect(mapState, mapDispatchToProps)(CoursesPage));