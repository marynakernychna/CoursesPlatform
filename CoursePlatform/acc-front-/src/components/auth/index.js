import AuthPage from './auth';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    startLoading,
    finishLoading,
    setAlert
 } from '../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        isAlert: stateRedux.generalReducer.isAlert,
        pageName: stateRedux.authReducer.pageName
    }
}

const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert
}

export default withRouter(connect(mapState, mapDispatchToProps)(AuthPage));