import Alerts from './alert';
import { connect } from 'react-redux';
import { closeAlert } from '../../actions/general';

const mapState = (stateRedux) => {
    return {
        alertInfo: stateRedux.generalReducer.alertInfo
    }
}
const mapDispatchToProps = {
    closeAlert
}

export default connect(mapState, mapDispatchToProps)(Alerts);