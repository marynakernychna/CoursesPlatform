import DataTable from './dataTable';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    startLoading,
    finishLoading,
    setAlert,
    openModal
} from '../../../actions/general';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        isAlert: stateRedux.generalReducer.isAlert,
        isModal: stateRedux.generalReducer.isModal,

        sectionName: stateRedux.panelReducer.sectionName
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert,
    openModal
}

export default withRouter(connect(mapState, mapDispatchToProps)(DataTable));