import RegistrationPage from './registration';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    startLoading,
    finishLoading,
    setAlert
 } from '../../../actions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        isAlert: stateRedux.generalReducer.isAlert
    }
}

const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert
}

export default withRouter(connect(mapState, mapDispatchToProps)(RegistrationPage));