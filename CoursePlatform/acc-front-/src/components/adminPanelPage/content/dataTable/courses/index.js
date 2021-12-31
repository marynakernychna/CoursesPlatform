import Courses from './courses';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    openModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../../../reduxActions/general/index';
import { 
    setCourses,
    clearTotalCount,
    setTotalCount,
    resetIsSortChangedStatus,
    changeCurrentPage 
} from '../../../../../reduxActions/panel/index';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        data: stateRedux.panelReducer.data,
        isSortingChanged: stateRedux.panelReducer.isSortingChanged
    }
}
const mapDispatchToProps = {
    openModal,
    startLoading,
    finishLoading,
    setAlert,
    setCourses,
    clearTotalCount,
    setTotalCount,
    resetIsSortChangedStatus,
    changeCurrentPage
}

export default withRouter(connect(mapState, mapDispatchToProps)(Courses));