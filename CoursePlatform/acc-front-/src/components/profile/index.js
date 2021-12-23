import Profile from "./profile";
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { 
    startLoading,
    finishLoading,
    setAlert
 } from '../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
    }
}

const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert
}

export default withRouter(connect(mapState, mapDispatchToProps)(Profile));