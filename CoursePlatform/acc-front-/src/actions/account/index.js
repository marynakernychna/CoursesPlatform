import * as types from "./types";

export const loginStarted = () => {
    return {
        type: types.LOGINSTARTED
    };
}

export const loginFailed = (error) => {
    return {
        type: types.LOGINFAILED,
        payload: error
    };
}

export const registrationStarted = () => {
    return {
        type: types.REGISTRATIONSTARTED
    };
}

export const registrationSuccess = () => {
    return {
        type: types.REGISTRATIONSUCCESS
    };
}

export const registrationFailed = (error) => {
    return {
        type: types.REGISTRATIONFAILED,
        payload: error
    };
}

export const setToken = (model) => {
    return {
        type: types.SETTOKEN,
        payload: model
    };
}

export const clearWarning = () => {
    return {
        type: types.CLEARWARNING
    };
}