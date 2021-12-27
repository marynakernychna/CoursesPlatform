import Profile from "./profile";
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert,
    openModal
} from '../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
        profileInfo: stateRedux.panelReducer.profileInfo,
        date: stateRedux.panelReducer.date
    }
}

const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    openModal
}

export default withRouter(connect(mapState, mapDispatchToProps)(Profile));