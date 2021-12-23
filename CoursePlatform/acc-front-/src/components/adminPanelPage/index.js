import AdminPanelPage from './adminPanel';
import { withRouter } from "react-router-dom";
import { connect } from 'react-redux';
import { 
    startLoading,
    finishLoading,
    setAlert,
    openModal
 } from '../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        isAlert: stateRedux.generalReducer.isAlert,
        isModal: stateRedux.generalReducer.isModal
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    openModal
}

export default withRouter(connect(mapState, mapDispatchToProps)(AdminPanelPage));
