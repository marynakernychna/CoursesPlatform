import Subscriptions from './subscriptions';
import { connect } from 'react-redux';
import { clearModal } from '../../../../../../../actions/general';

const mapState = (stateRedux) => {
    return {
        modal: stateRedux.panelsReducer.modal
    }
}
const mapDispatchToProps = {
    clearModal
}

export default connect(mapState, mapDispatchToProps)(Subscriptions);