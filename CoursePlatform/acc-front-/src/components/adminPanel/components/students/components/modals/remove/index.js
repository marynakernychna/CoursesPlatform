import Remove from './remove';
import { connect } from 'react-redux';
import { clearModal } from '../../../../../../../actions/general';
import {
    removeStudentStarted,
    removeStudentSuccess,
    removeStudentFailed
} from '../../../../../../../actions/users';

const mapState = (stateRedux) => {
    return {
        modal: stateRedux.panelsReducer.modal
    }
}
const mapDispatchToProps = {
    removeStudentStarted,
    removeStudentSuccess,
    removeStudentFailed,
    clearModal
}

export default connect(mapState, mapDispatchToProps)(Remove);