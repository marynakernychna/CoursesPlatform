import * as types from "./types";

export const logOut = () => {
    return {
        type: types.LOGOUT
    };
}

export const startLoading = () => {
    return {
        type: types.STARTLOADING
    }
}

export const finishLoading = () => {
    return {
        type: types.FINISHLOADING
    }
}

export const setAccess = (token) => {
    return {
        type: types.SETACCESS,
        payload: token
    }
}

export const setAlert = (model) => {
    return {
        type: types.SETALERT,
        payload: model
    }
}

export const closeAlert = () => {
    return {
        type: types.CLOSEALERT
    }
}

export const openModal = (modal) => {
    return {
        type: types.OPENMODAL,
        payload: modal
    };
}

export const closeModal = () => {
    return {
        type: types.CLOSEMODAL
    };
}

export const updateAccess = (token) => {
    return {
        type: types.UPDATEACCESS,
        payload: token
    }
}