import Students from './students';
import { connect } from 'react-redux';
import {
    getStudentsStarted,
    getStudentsSuccess,
    getStudentsFailed,
    removeStudentStarted,
    removeStudentSuccess,
    removeStudentFailed,
    editStudentStarted,
    editStudentSuccess,
    editStudentFailed,
} from '../../../../actions/users/index';

import { showModal } from '../../../../actions/general/index'

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.panelsReducer.loading,
        isModalOpen: stateRedux.panelsReducer.isModalOpen,
        isAlert: stateRedux.panelsReducer.isAlert,
        students: stateRedux.panelsReducer.students
    }
}
const mapDispatchToProps = {
    getStudentsStarted,
    getStudentsSuccess,
    getStudentsFailed,
    removeStudentStarted,
    removeStudentSuccess,
    removeStudentFailed,
    editStudentStarted,
    editStudentSuccess,
    editStudentFailed,
    showModal
}

export default connect(mapState, mapDispatchToProps)(Students);
