import * as types from "./types";

export const setSearchText = (text) => {
    return {
        type: types.SET_SEACRCH_TEXT,
        payload: text
    };
}

export const setProfileInfo = (info) => {
    return {
        type: types.SET_PROFILE_INFO,
        payload: info
    };
}

export const setDate = (date) => {
    return {
        type: types.SET_DATE,
        payload: date
    };
}

export const changeSection = (sectionName) => {
    return {
        type: types.CHANGESECTION,
        payload: sectionName
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

export const updateTableData = (data) => {
    return {
        type: types.UPDATE_TABLE_DATA,
        payload: data
    }
}

export const setCourses = (courses) => {
    return {
        type: types.SET_COURSES,
        payload: courses
    }
}

export const setStudents = (students) => {
    return {
        type: types.SET_STUDENTS,
        payload: students
    }
}

export const removeCourse = (model) => {
    return {
        type: types.REMOVE_COURSE,
        payload: model
    }
}

export const removeStudent = (model) => {
    return {
        type: types.REMOVE_STUDENT,
        payload: model
    }
}