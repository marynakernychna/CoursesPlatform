import Email from './email';
import { withRouter } from "react-router-dom";
import { connect } from 'react-redux';
import {
    setAlert,
    startLoading,
    finishLoading
} from '../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert
}

export default withRouter(connect(mapState, mapDispatchToProps)(Email));