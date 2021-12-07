import * as types from "./types";

export const logOut = () => {
    return {
        type: types.LOGOUT
    };
}

export const showModal = (data) => {
    return {
        type: types.SHOWMODAL,
        payload: data
    };
}

export const clearModal = (data) => {
    return {
        type: types.CLEARMODAL,
        payload: data
    };
}

/// 

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

export const changeSection = (sectionName) => {
    return {
        type: types.CHANGESECTION,
        payload: sectionName
    };
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

export const changeElementsOnPageCount = (onPage) => {
    return {
        type: types.CHANGE_ELEMENTS_ON_PAGE_COUNT,
        payload: onPage
    }
}

export const changeElementsSortDirection = (direction) => {
    return {
        type: types.CHANGE_ELEMENTS_SORT_DIRECTION,
        payload: direction
    }
}

export const changeElementsSortBy = (sortBy) => {
    return {
        type: types.CHANGE_ELEMENTS_SORT_BY,
        payload: sortBy
    }
}

export const changeCurrentPage = (page) => {
    return {
        type: types.CHANGE_CURRENT_PAGE,
        payload: page
    }
}

export const setTotalCount = (count) => {
    return {
        type: types.SET_TOTAL_COUNT,
        payload: count
    }
}

export const resetIsSortChangedStatus = () => {
    return {
        type: types.RESET_IS_SORT_CHANGED
    };
}

export const resetIsElementsChanged = () => {
    return {
        type: types.RESET_IS_ELEMENTS_CHANGED
    }
}

export const setIsElementsChanged = () => {
    return {
        type: types.SET_IS_ELEMENTS_CHANGED
    }
}

export const clearTotalCount = () => {
    return {
        type: types.CLEAR_TOTAL_COUNT
    }
}

export const updateCourses = (data) => {
    return {
        type: types.UPDATE_COURSES,
        payload: data
    }
}

export const setCourses = (courses) => {
    return {
        type: types.SET_COURSES,
        payload: courses
    }
}