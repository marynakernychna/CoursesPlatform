import Unsubscribe from './unsubscribe';
import { connect } from 'react-redux';
import { 
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    setIsElementsChanged
 } from '../../../actions/general';

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