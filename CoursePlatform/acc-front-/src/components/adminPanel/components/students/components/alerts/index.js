import Alerts from './alerts';
import { connect } from 'react-redux';
import {
    clearWarning,
    clearInfo
} from '../../../../../../actions/users/index';

const mapState = (stateRedux) => {
    return {
        warning: stateRedux.panelsReducer.warning,
        info: stateRedux.panelsReducer.info,
    }
}
const mapDispatchToProps = {
    clearWarning,
    clearInfo
}

export default connect(mapState, mapDispatchToProps)(Alerts);