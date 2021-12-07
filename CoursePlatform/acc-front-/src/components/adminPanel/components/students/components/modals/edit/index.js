import Edit from './edit';
import { connect } from 'react-redux';
import { clearModal } from '../../../../../../../actions/general';
import {
    editStudentStarted,
    editStudentSuccess,
    editStudentFailed
} from '../../../../../../../actions/users';

const mapState = (stateRedux) => {
    return {
        modal: stateRedux.panelsReducer.modal
    }
}
const mapDispatchToProps = {
    editStudentStarted,
    editStudentSuccess,
    editStudentFailed,
    clearModal
}

export default connect(mapState, mapDispatchToProps)(Edit);