import Modals from './modals';
import { connect } from 'react-redux';

const mapState = (stateRedux) => {
    return {
        modalInfo: stateRedux.generalReducer.modalInfo
    }
}

export default connect(mapState)(Modals);