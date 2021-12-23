import Unsubscribe from './unsubscribe';
import { connect } from 'react-redux';
import { 
    closeModal,
    startLoading,
    finishLoading,
    setAlert
 } from '../../../reduxActions/general';
 import { setIsElementsChanged } from '../../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    setIsElementsChanged
}

export default connect(mapState, mapDispatchToProps)(Unsubscribe);