import AddCourse from './addCourse';
import { connect } from 'react-redux';
import {
    addCourseStarted,
    addCourseSuccess,
    addCourseFailed,
    clearWarning,
    clearInfo
} from '../../../../actions/courses/index';

const mapState = (stateRedux) => {
    return {
        warning: stateRedux.panelsReducer.warning,
        info: stateRedux.panelsReducer.info,
        loading: stateRedux.panelsReducer.loading
    }
}
const mapDispatchToProps = {
    addCourseStarted,
    addCourseSuccess,
    addCourseFailed,
    clearWarning,
    clearInfo
}

export default connect(mapState, mapDispatchToProps)(AddCourse);
