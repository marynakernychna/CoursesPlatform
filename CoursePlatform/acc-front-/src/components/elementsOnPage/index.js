import ElementsOnPage from './elementsOnPage';
import { connect } from 'react-redux';
import { changeElementsOnPageCount } from '../../reduxActions/panel';

const mapState = (stateRedux) => {
    return {
        elementsOnPage: stateRedux.panelReducer.elementsOnPage,
        totalElementsCount: stateRedux.panelReducer.totalElementsCount
    }
}
const mapDispatchToProps = {
    changeElementsOnPageCount
}

export default connect(mapState, mapDispatchToProps)(ElementsOnPage);