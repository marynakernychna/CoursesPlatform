import EditProfile from './edit';
import { connect } from 'react-redux';
import {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    logOut
} from '../../../reduxActions/general';
import { setProfileInfo, setDate     } from '../../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    logOut,
    setProfileInfo, setDate
}

export default connect(mapState, mapDispatchToProps)(EditProfile);