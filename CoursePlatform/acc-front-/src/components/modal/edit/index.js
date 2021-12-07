import Edit from './edit';
import { connect } from 'react-redux';
import { 
    closeModal,
    startLoading,
    finishLoading,
    setAlert,
    updateCourses
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
    updateCourses
}

export default connect(mapState, mapDispatchToProps)(Edit);