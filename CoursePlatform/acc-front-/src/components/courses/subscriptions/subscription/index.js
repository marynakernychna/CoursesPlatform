import Subscription from './subscription';
import { connect } from 'react-redux';
import { withRouter } from "react-router-dom";
import { openModal } from '../../../../reduxActions/general';

const mapState = (stateRedux) => {
    return {
    }
}
const mapDispatchToProps = {
    openModal
}

export default withRouter(connect(mapState, mapDispatchToProps)(Subscription));