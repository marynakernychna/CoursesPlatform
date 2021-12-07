import AdminPanel from './adminPanel';
import { withRouter } from "react-router-dom";
import { connect } from 'react-redux';

const mapState = (stateRedux) => {
    return {
        sectionName: stateRedux.panelReducer.sectionName
    }
}

export default withRouter(connect(mapState)(AdminPanel));
