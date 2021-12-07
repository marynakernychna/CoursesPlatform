import jwt from 'jsonwebtoken';
import { roles } from '../actions/account/roles';
import * as types from "../actions/general/types";
import { CHANGESECTION, LOGOUT } from '../actions/general/types';

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

            var token = action.payload;

            var role = jwt.decode(token).roles;

            localStorage.setItem("authToken", token);

            return {
                ...state,
                isAuthUser: true,
                role: role
            };
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

        case LOGOUT: {
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