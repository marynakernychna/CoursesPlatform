import * as coursesTypes from '../../actions/courses/types'
import * as usersTypes from '../../actions/users/types'
import { LOGOUT, CHANGESECTION, SHOWMODAL, CLEARMODAL } from '../../actions/general/types'
// import { COURSESCHANGE } from './sideMenu/sectionNames'
import * as modalTypes from '../adminPanel/components/students/components/modals/modalTypes'
import students from '../adminPanel/components/students'

const intialState = {
    sectionName: "Courses",
    // adminSectionName: COURSESCHANGE,
    warning: "",
    info: "",
    loading: false,
    modal: {},
    students: [],
    isModalOpen: false,
    isAlert: false
}

const panelsReducer = (state = intialState, action) => {

    switch (action.type) {

        case SHOWMODAL:
            {
                return {
                    ...state,
                    modal: action.payload,
                    isModalOpen: true
                }
            }
        case CLEARMODAL:
            {
                if (action.payload != undefined &&
                    action.payload.email != undefined) {

                    const { newInfo, email } = action.payload;

                    const students = state.students.map((item, index) => {
                        if (item.email !== email) {
                            return item;
                        }
                    
                        return {
                          ...item,
                          ...newInfo
                        }
                      })

                    return {
                        ...state,
                        modal: {},
                        isModalOpen: false,
                        students
                    }
                }

                return {
                    ...state,
                    modal: {},
                    isModalOpen: false
                }
            }

        case coursesTypes.GETCOURSESSTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.GETCOURSESFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.GETCOURSESSUCCESS:
            {
                return {
                    ...state,
                    loading: false
                }
            }

        case coursesTypes.GETUSERCOURSESSTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.GETUSERCOURSESSUCCESS:
            {
                return {
                    ...state,
                    loading: false
                }
            }
        case coursesTypes.GETUSERCOURSESFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case coursesTypes.ENROLLINCOURSESTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.ENROLLINCOURSESUCCESS:
            {
                return {
                    ...state,
                    info: "You have successfully enrolled ! The course is available in the <My courses> section .",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.ENROLLINCOURSEFAILED:
            {
                return {
                    ...state,
                    warning: action.payload,
                    loading: false,
                    isAlert: true
                }
            }

        case coursesTypes.CLEARWARNING:
            {
                return {
                    ...state,
                    warning: "",
                    isAlert: false
                };
            }
        case coursesTypes.CLEARINFO:
            {
                return {
                    ...state,
                    info: "",
                    isAlert: false
                };
            }

        case coursesTypes.UNSUBSCRIBESTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.UNSUBSCRIBESUCCESS:
            {
                return {
                    ...state,
                    info: "You have unsubscribed successfully !",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.UNSUBSCRIBEFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case coursesTypes.REMOVECOURSESTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.REMOVECOURSESUCCESS:
            {
                return {
                    ...state,
                    info: "You have removed successfully !",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.REMOVECOURSEFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case coursesTypes.EDITCOURSESTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.EDITCOURSESUCCESS:
            {
                return {
                    ...state,
                    info: "You have edited course successfully !",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.EDITCOURSEFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case coursesTypes.ADDCOURSESTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case coursesTypes.ADDCOURSEFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }
        case coursesTypes.ADDCOURSESUCCESS:
            {
                return {
                    ...state,
                    info: "You have added course successfully !",
                    loading: false,
                    isAlert: true
                }
            }

        // users

        case usersTypes.GETSTUDENTSSTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case usersTypes.GETSTUDENTSFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }
        case usersTypes.GETSTUDENTSSUCCESS:
            {
                return {
                    ...state,
                    loading: false,
                    students: action.payload
                }
            }

        case usersTypes.CLEARWARNING:
            {
                return {
                    ...state,
                    warning: "",
                    isAlert: true
                };
            }
        case usersTypes.CLEARINFO:
            {
                return {
                    ...state,
                    info: "",
                    isAlert: true
                };
            }

        case usersTypes.REMOVESTUDENTSTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case usersTypes.REMOVESTUDENTSUCCESS:
            {
                return {
                    ...state,
                    info: "You have removed user successfully !",
                    loading: false,
                    isAlert: true
                }
            }
        case usersTypes.REMOVESTUDENTFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case usersTypes.EDITSTUDENTSTARTED:
            {
                return {
                    ...state,
                    loading: true
                }
            }
        case usersTypes.EDITSTUDENTSUCCESS:
            {
                return {
                    ...state,
                    info: "You have edited user successfully !",
                    loading: false,
                    isAlert: true
                }
            }
        case usersTypes.EDITSTUDENTFAILED:
            {
                return {
                    ...state,
                    warning: "Something went wrong, try again !",
                    loading: false,
                    isAlert: true
                }
            }

        case CHANGESECTION: {
            return {
                ...state,
                sectionName: action.payload,
                adminSectionName: action.payload,
                warning: "",
                info: "",
                loading: false,
                isAlert: false
            };
        }

        case LOGOUT: {
            return {
                ...state,
                sectionName: "",
                warning: "",
                info: "",
                loading: false,
                modal: {},
                isModalOpen: false,
                // adminSectionName: COURSESCHANGE,
                isAlert: false
            };
        }

        default: {
            return state;
        }
    }
}

export default panelsReducer;