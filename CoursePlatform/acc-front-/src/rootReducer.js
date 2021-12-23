import { combineReducers } from 'redux';

import generalReducer from './reducers/general';
import panelReducer from './reducers/panelReducer';
import authReducer from './reducers/auth';

export default combineReducers({
    authReducer: authReducer,
    generalReducer: generalReducer,
    panelReducer: panelReducer
});