import RegistrationPage from './registration';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert
} from '../../../reduxActions/general';
import { changeAuthPage } from '../../../reduxActions/auth/index';

const mapState = (stateRedux) => {
    return {
    }
}

const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    changeAuthPage
}

export default withRouter(connect(mapState, mapDispatchToProps)(RegistrationPage));