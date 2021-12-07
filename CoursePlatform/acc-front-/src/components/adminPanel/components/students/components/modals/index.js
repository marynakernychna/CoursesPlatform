import Modals from './modals';
import { connect } from 'react-redux';

const mapState = (stateRedux) => {
    return {
        modal: stateRedux.panelsReducer.modal
    }
}

export default connect(mapState)(Modals);