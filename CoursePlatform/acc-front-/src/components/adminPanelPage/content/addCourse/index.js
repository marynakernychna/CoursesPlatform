import AddCourse from './addCourse';
import { connect } from 'react-redux';
import {
    startLoading,
    finishLoading,
    setAlert
} from '../../../../reduxActions/general/index';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    startLoading,
    finishLoading,
    setAlert
}

export default connect(mapState, mapDispatchToProps)(AddCourse);