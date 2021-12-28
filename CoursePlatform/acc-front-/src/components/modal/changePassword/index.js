import ChangePassword from './change';
import { connect } from 'react-redux';
import {
    closeModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert
}

export default connect(mapState, mapDispatchToProps)(ChangePassword);