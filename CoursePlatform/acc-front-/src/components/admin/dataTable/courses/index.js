import Courses from './courses';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    openModal,
    startLoading,
    finishLoading,
    setAlert,
    setCourses
} from '../../../../actions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        courses: stateRedux.panelReducer.courses
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