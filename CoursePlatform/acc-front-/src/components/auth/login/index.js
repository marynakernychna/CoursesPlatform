import LoginPage from './login';
import { connect } from 'react-redux';
import {
    startLoading,
    finishLoading,
    setAccess,
    setAlert
} from '../../../reduxActions/general';
import { changeAuthPage } from '../../../reduxActions/auth/index';

const mapState = (stateRedux) => {
    return {
        role: stateRedux.generalReducer.role
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAccess,
    setAlert,
    changeAuthPage
}

export default connect(mapState, mapDispatchToProps)(LoginPage);