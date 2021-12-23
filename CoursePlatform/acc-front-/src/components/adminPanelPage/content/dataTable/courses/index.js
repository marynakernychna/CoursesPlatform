import Courses from './courses';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    openModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../../../reduxActions/general/index';
import { setCourses } from '../../../../../reduxActions/panel/index';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        data: stateRedux.panelReducer.data
    }
}
const mapDispatchToProps = {
    openModal,
    startLoading,
    finishLoading,
    setAlert,
    setCourses
}

export default withRouter(connect(mapState, mapDispatchToProps)(Courses));