import LoginPage from './login';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAccess,
    setAlert
} from '../../../actions/general';

const mapState = (stateRedux) => {
    return {
        role: stateRedux.generalReducer.role,
        isAlert: stateRedux.generalReducer.isAlert,
        loading: stateRedux.generalReducer.loading
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAccess,
    setAlert
}

export default withRouter(connect(mapState, mapDispatchToProps)(LoginPage));