import Edit from './edit';
import { connect } from 'react-redux';
import {
    closeModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../reduxActions/general';
import { updateTableData } from '../../../reduxActions/panel/index';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    updateTableData
}

export default connect(mapState, mapDispatchToProps)(Edit);