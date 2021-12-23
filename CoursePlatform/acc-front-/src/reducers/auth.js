import { pagesNames } from "../constants/pagesNames";
import * as types from '../reduxActions/auth/types';

const intialState = {
    pageName: pagesNames.LOGIN
}

const authReducer = (state = intialState, action) => {

    switch (action.type) {

        case types.CHANGE_AUTH_PAGE: {
            return {
                ...state,
                pageName: action.payload
            }
        }

        default: {
            return state;
        }
    }
}

export default authReducer;