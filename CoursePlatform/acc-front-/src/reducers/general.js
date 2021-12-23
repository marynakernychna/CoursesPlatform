import jwt from 'jsonwebtoken';
import { roles } from '../constants/roles';
import * as types from '../reduxActions/general/types';
import { CHANGESECTION } from '../reduxActions/panel/types';

const intialState = {
    role: roles.GUEST,
    isAuthUser: false,
    loading: false,
    isAlert: false,
    isModal: false,
    alertInfo: {},
    modalInfo: {}
}

const generalReducer = (state = intialState, action) => {
    switch (action.type) {

        case CHANGESECTION: {
            return {
                ...state,
                isAlert: false,
                alertInfo: {}
            }
        }

        case types.STARTLOADING: {
            return {
                ...state,
                loading: true
            };
        }

        case types.FINISHLOADING: {
            return {
                ...state,
                loading: false
            };
        }

        case types.SETACCESS: {

            const { accessToken, refreshToken } = action.payload;

            var role = jwt.decode(accessToken).roles;

            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", refreshToken);

            return {
                ...state,
                isAuthUser: true,
                role: role
            };
        }

        case types.UPDATEACCESS: {

            localStorage.removeItem("accessToken");

            localStorage.setItem("accessToken", action.payload);

            return state;
        }

        case types.SETALERT: {

            return {
                ...state,
                isAlert: true,
                alertInfo: action.payload
            };
        }

        case types.CLOSEALERT: {

            return {
                ...state,
                isAlert: false,
                alertInfo: {}
            };
        }

        case types.OPENMODAL: {

            return {
                ...state,
                isModal: true,
                modalInfo: action.payload
            }
        }

        case types.CLOSEMODAL: {

            return {
                ...state,
                isModal: false,
                modalInfo: {}
            }
        }

        case types.LOGOUT: {

            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");

            return {
                ...state,
                role: roles.GUEST,
                isAuthUser: false,
                loading: false,
                isAlert: false,
                isModal: false,
                alertInfo: {},
                modalInfo: {}
            }
        }

        default: {
            return state;
        }
    }
}

export default generalReducer;