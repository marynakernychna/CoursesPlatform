import Students from './students';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    openModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../../../reduxActions/general';
import {
    setStudents,
    clearTotalCount,
    setTotalCount,
    resetIsSortChangedStatus
} from '../../../../../reduxActions/panel/index';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        data: stateRedux.panelReducer.data,
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        currentPage: stateRedux.panelReducer.currentPage,
        isSortingChanged: stateRedux.panelReducer.isSortingChanged
    }
}
const mapDispatchToProps = {
    openModal,
    startLoading,
    finishLoading,
    setAlert,
    setStudents,
    clearTotalCount,
    setTotalCount,
    resetIsSortChangedStatus
}

export default withRouter(connect(mapState, mapDispatchToProps)(Students));