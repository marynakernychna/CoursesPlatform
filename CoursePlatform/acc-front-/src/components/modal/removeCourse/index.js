import Remove from './remove';
import { connect } from 'react-redux';
import {
    closeModal,
    startLoading,
    finishLoading,
    setAlert
} from '../../../reduxActions/general';
import { removeCourse } from '../../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    removeCourse
}

export default connect(mapState, mapDispatchToProps)(Remove);