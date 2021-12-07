import * as types from "./types";

export const getCoursesStarted = () => {
    return {
        type: types.GETCOURSESSTARTED
    };
}

export const getCoursesFailed = () => {
    return {
        type: types.GETCOURSESFAILED
    };
}

export const getCoursesSuccess = () => {
    return {
        type: types.GETCOURSESSUCCESS
    };
}

export const getUserCoursesStarted = () => {
    return {
        type: types.GETUSERCOURSESSTARTED
    };
}

export const getUserCoursesSuccess = () => {
    return {
        type: types.GETUSERCOURSESSUCCESS
    };
}

export const getUserCoursesFailed = () => {
    return {
        type: types.GETUSERCOURSESFAILED
    };
}

export const enrollInCourseStarted = () => {
    return {
        type: types.ENROLLINCOURSESTARTED
    };
}

export const enrollInCourseSuccess = () => {
    return {
        type: types.ENROLLINCOURSESUCCESS
    };
}

export const enrollInCourseFailed = (message) => {
    return {
        type: types.ENROLLINCOURSEFAILED,
        payload: message
    };
}

export const unsubscribeFromCourseStarted = () => {
    return {
        type: types.UNSUBSCRIBESTARTED
    };
}

export const unsubscribeFromCourseSuccess = () => {
    return {
        type: types.UNSUBSCRIBESUCCESS
    };
}

export const unsubscribeFromCourseFailed = () => {
    return {
        type: types.UNSUBSCRIBEFAILED
    };
}

export const removeCourseStarted = () => {
    return {
        type: types.REMOVECOURSESTARTED
    };
}

export const removeCourseSuccess = () => {
    return {
        type: types.REMOVECOURSESUCCESS
    };
}

export const removeCourseFailed = () => {
    return {
        type: types.REMOVECOURSEFAILED
    };
}

export const editCourseStarted = () => {
    return {
        type: types.EDITCOURSESTARTED
    };
}

export const editCourseSuccess = () => {
    return {
        type: types.EDITCOURSESUCCESS
    };
}

export const editCourseFailed = () => {
    return {
        type: types.EDITCOURSEFAILED
    };
}

export const addCourseStarted = () => {
    return {
        type: types.ADDCOURSESTARTED
    };
}

export const addCourseFailed = () => {
    return {
        type: types.ADDCOURSEFAILED
    };
}

export const addCourseSuccess = () => {
    return {
        type: types.ADDCOURSESUCCESS
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