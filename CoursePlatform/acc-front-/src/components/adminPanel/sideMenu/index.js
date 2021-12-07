import AdminSideMenu from './adminSideMenu';
import { connect } from 'react-redux';
import {
    logOut,
    changeSection
} from "../../../actions/general/index"

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    logOut,
    changeSection
}

export default connect(mapState, mapDispatchToProps)(AdminSideMenu);