import StudentSideMenu from './studentsSideMenu';
import { connect } from 'react-redux';
import {
    logOut,
    changeSection
} from '../../../actions/general/index';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    logOut,
    changeSection
}

export default connect(mapState, mapDispatchToProps)(StudentSideMenu);