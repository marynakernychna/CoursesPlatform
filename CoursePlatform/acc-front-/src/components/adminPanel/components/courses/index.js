import Courses from './courses';
import { connect } from 'react-redux';
import {
    getCoursesStarted,
    getCoursesSuccess,
    getCoursesFailed,
    removeCourseStarted,
    removeCourseSuccess,
    removeCourseFailed,
    editCourseStarted,
    editCourseSuccess,
    editCourseFailed,
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
    getCoursesStarted,
    getCoursesSuccess,
    getCoursesFailed,
    removeCourseStarted,
    removeCourseSuccess,
    removeCourseFailed,
    editCourseStarted,
    editCourseSuccess,
    editCourseFailed,
    clearWarning,
    clearInfo
}

export default connect(mapState, mapDispatchToProps)(Courses);
