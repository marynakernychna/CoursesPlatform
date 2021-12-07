import Enroll from './enroll';
import { connect } from 'react-redux';
import { 
    closeModal,
    startLoading,
    finishLoading,
    setAlert
 } from '../../../actions/general';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    closeModal,
    startLoading,
    finishLoading,
    setAlert
}

export default connect(mapState, mapDispatchToProps)(Enroll);