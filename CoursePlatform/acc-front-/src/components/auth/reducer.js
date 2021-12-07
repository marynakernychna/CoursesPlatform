import * as types from '../../actions/account/types';
import { roles } from '../../actions/account/roles';
import { LOGOUT } from '../../actions/general/types';

const intialState = {
    isAuthUser: false,
    role: roles.GUEST,
    warning: "",
    loading: false
}

const authReducer = (state = intialState, action) => {
    switch (action.type) {

        case types.CLEARWARNING: {
            return {
                ...state,
                warning: ""
            };
        }
        
        case LOGOUT: {
            localStorage.removeItem("authToken");

            return {
                ...state,
                isAuthUser: false,
                role: roles.GUEST,
                warning: "",
                loading: false
            };
        }

        default: {
            return state;
        }
    }
}

export default authReducer;