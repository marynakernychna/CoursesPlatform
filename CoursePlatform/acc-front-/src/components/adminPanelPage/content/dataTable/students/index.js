import Students from './students';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import {
    openModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../../../reduxActions/general';
import { setStudents } from '../../../../../reduxActions/panel/index';

const mapState = (stateRedux) => {
    return {
        loading: stateRedux.generalReducer.loading,
        data: stateRedux.panelReducer.data
    }
}
const mapDispatchToProps = {
    openModal,
    startLoading,
    finishLoading,
    setAlert,
    setStudents
}

export default withRouter(connect(mapState, mapDispatchToProps)(Students));