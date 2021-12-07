import {
    CHANGESECTION,
    RESET_IS_SORT_CHANGED,
    CHANGE_ELEMENTS_ON_PAGE_COUNT,
    CHANGE_ELEMENTS_SORT_DIRECTION,
    CHANGE_ELEMENTS_SORT_BY,
    CHANGE_CURRENT_PAGE,
    SET_TOTAL_COUNT,
    RESET_IS_ELEMENTS_CHANGED,
    SET_IS_ELEMENTS_CHANGED,
    LOGOUT,
    CLEAR_TOTAL_COUNT,
    UPDATE_COURSES,
    SET_COURSES
} from "../actions/general/types";
import { sectionsNames } from "../actions/general/sectionsNames";
import { sortDirectionTypes } from "../actions/general/sortDirectionTypes";
import { sortByTypes } from "../actions/general/sortByTypes";
import { elementsOnPage } from "../actions/general/elementsOnPageCount";

const intialState = {
    sectionName: sectionsNames.COURSES,

    elementsOnPage: elementsOnPage[10],
    sortDirection: sortDirectionTypes.ASC,
    sortBy: sortByTypes.TITLE,
    currentPage: 1,
    totalElementsCount: 0,

    isSortingChanged: false,
    isElementsChanged: false,

    courses: []
}

const panelReducer = (state = intialState, action) => {
    switch (action.type) {

        case CHANGESECTION: {
            return {
                ...state,
                sectionName: action.payload,
                elementsOnPage: elementsOnPage[10],
                sortDirection: sortDirectionTypes.ASC,
                sortBy: sortByTypes.TITLE,
                currentPage: 1,
                isSortingChanged: false,
                isElementsChanged: false
            }
        }

        case RESET_IS_SORT_CHANGED: {
            return {
                ...state,
                isSortingChanged: false
            }
        }

        case CHANGE_ELEMENTS_ON_PAGE_COUNT: {
            return {
                ...state,
                elementsOnPage: action.payload,
                isSortingChanged: true
            }
        }

        case CHANGE_ELEMENTS_SORT_DIRECTION: {
            return {
                ...state,
                sortDirection: action.payload,
                isSortingChanged: true
            }
        }

        case CHANGE_ELEMENTS_SORT_BY: {
            return {
                ...state,
                sortBy: action.payload,
                isSortingChanged: true
            }
        }

        case CHANGE_CURRENT_PAGE: {
            return {
                ...state,
                currentPage: action.payload,
                isSortingChanged: true
            }
        }

        case SET_TOTAL_COUNT: {
            return {
                ...state,
                totalElementsCount: action.payload
            }
        }

        case SET_IS_ELEMENTS_CHANGED: {
            return {
                ...state,
                isElementsChanged: true
            }
        }

        case RESET_IS_ELEMENTS_CHANGED: {
            return {
                ...state,
                isElementsChanged: false
            }
        }

        case CLEAR_TOTAL_COUNT: {
            return {
                ...state,
                totalElementsCount: 0
            }
        }

        case UPDATE_COURSES: {
            
            const courses = state.courses.map((item, index) => {
                if (item.key !== action.payload.key) {
                    return item;
                }

                return {
                    ...item,
                    ...action.payload.newData
                }
            });

            return {
                ...state,
                courses
            }
        }

        case SET_COURSES: {
            return {
                ...state,
                courses: action.payload
            }
        }

        case LOGOUT: {
            return {
                ...state,
                sectionName: sectionsNames.COURSES,
                elementsOnPage: elementsOnPage[10],
                sortDirection: sortDirectionTypes.ASC,
                sortBy: sortByTypes.TITLE,
                currentPage: 1,
                totalElementsCount: 0,
                isSortingChanged: false,
                isElementsChanged: false
            }
        }

        default: {
            return state;
        }
    }
}

export default panelReducer;