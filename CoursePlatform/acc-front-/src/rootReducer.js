import { combineReducers } from 'redux';

import authReducer from './components/auth/reducer';
import panelsReducer from './components/studentPanel/reducer';
import generalReducer from './reducers/general';
import panelReducer from './reducers/panelReducer';

export default combineReducers({
    generalReducer: generalReducer,
    panelReducer: panelReducer,
    authReducer: authReducer,
    panelsReducer: panelsReducer
});