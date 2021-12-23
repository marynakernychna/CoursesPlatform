import RemoveStudent from './remove';
import { connect } from 'react-redux';
import { 
    closeModal,
    startLoading,
    finishLoading,
    setAlert
 } from '../../../reduxActions/general';
 import { removeStudent } from '../../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    removeStudent
}

export default connect(mapState, mapDispatchToProps)(RemoveStudent);