import * as types from "./types";

export const getStudentsStarted = () => {
    return {
        type: types.GETSTUDENTSSTARTED
    };
}

export const getStudentsFailed = () => {
    return {
        type: types.GETSTUDENTSFAILED
    };
}

export const getStudentsSuccess = (data) => {
    return {
        type: types.GETSTUDENTSSUCCESS,
        payload: data
    };
}

export const removeStudentStarted = () => {
    return {
        type: types.REMOVESTUDENTSTARTED
    };
}

export const removeStudentSuccess = () => {
    return {
        type: types.REMOVESTUDENTSUCCESS
    };
}

export const removeStudentFailed = () => {
    return {
        type: types.REMOVESTUDENTFAILED
    };
}

export const editStudentStarted = () => {
    return {
        type: types.EDITSTUDENTSTARTED
    };
}

export const editStudentSuccess = () => {
    return {
        type: types.EDITSTUDENTSUCCESS
    };
}

export const editStudentFailed = () => {
    return {
        type: types.EDITSTUDENTFAILED
    };
}

export const clearWarning = () => {
    return {
        type: types.CLEARWARNING
    };
}

export const clearInfo = () => {
    return {
        type: types.CLEARINFO
    };
}