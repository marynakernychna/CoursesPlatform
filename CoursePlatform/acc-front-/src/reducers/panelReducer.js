import * as types from "../reduxActions/panel/types";
import { LOGOUT } from "../reduxActions/general/types";
import { sectionsNames } from "../constants/sectionsNames";
import { sortDirectionTypes } from "../constants/sortDirectionTypes";
import { sortByTypes } from "../constants/sortByTypes";
import { elementsOnPage } from '../constants/elementsOnPageCount';

const intialState = {
    sectionName: sectionsNames.COURSES,

    elementsOnPage: elementsOnPage[10],
    sortDirection: sortDirectionTypes.ASC,
    sortBy: sortByTypes.TITLE,
    currentPage: 1,
    totalElementsCount: 0,

    isSortingChanged: false,
    isElementsChanged: false,

    data: [],
    profileInfo: {},
    date: undefined
}

const panelReducer = (state = intialState, action) => {
    switch (action.type) {

        case types.CHANGESECTION: {
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

        case types.SET_PROFILE_INFO: {

            return {
                ...state,
                profileInfo: action.payload
            }
        }

        case types.SET_DATE: {

            return {
                ...state,
                date: action.payload
            }
        }

        case types.RESET_IS_SORT_CHANGED: {
            return {
                ...state,
                isSortingChanged: false
            }
        }

        case types.CHANGE_ELEMENTS_ON_PAGE_COUNT: {
            return {
                ...state,
                elementsOnPage: action.payload,
                isSortingChanged: true,
                currentPage: 1
            }
        }

        case types.CHANGE_ELEMENTS_SORT_DIRECTION: {
            return {
                ...state,
                sortDirection: action.payload,
                isSortingChanged: true
            }
        }

        case types.CHANGE_ELEMENTS_SORT_BY: {
            return {
                ...state,
                sortBy: action.payload,
                isSortingChanged: true
            }
        }

        case types.CHANGE_CURRENT_PAGE: {
            return {
                ...state,
                currentPage: action.payload,
                isSortingChanged: true
            }
        }

        case types.SET_TOTAL_COUNT: {
            return {
                ...state,
                totalElementsCount: action.payload
            }
        }

        case types.SET_IS_ELEMENTS_CHANGED: {
            return {
                ...state,
                isElementsChanged: true
            }
        }

        case types.RESET_IS_ELEMENTS_CHANGED: {
            return {
                ...state,
                isElementsChanged: false
            }
        }

        case types.CLEAR_TOTAL_COUNT: {
            return {
                ...state,
                totalElementsCount: 0
            }
        }

        case types.UPDATE_TABLE_DATA: {

            const data = state.data.map((item, index) => {
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
                data
            }
        }

        case types.REMOVE_COURSE: {

            var elementIndex = state.data.indexOf(action.payload);

            state.data.splice(elementIndex, 1);

            return {
                ...state,
                data: [...state.data]
            }
        }

        case types.REMOVE_STUDENT: {

            var elementIndex = state.data.indexOf(action.payload);

            state.data.splice(elementIndex, 1);

            return {
                ...state,
                data: [...state.data]
            }
        }

        case types.SET_COURSES: {
            return {
                ...state,
                data: action.payload
            }
        }

        case types.SET_STUDENTS: {
            return {
                ...state,
                data: action.payload
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
                isElementsChanged: false,
                data: [],
                profileInfo: undefined
            }
        }

        default: {
            return state;
        }
    }
}

export default panelReducer;