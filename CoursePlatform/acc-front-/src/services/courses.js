import axios from "axios";
import store from '../index';
import { logOut, finishLoading, setAlert, updateAccess } from "../reduxActions/general";
import authService from "./auth";
import { alertTypes } from "../components/alert/types";

const URL = "https://localhost:5001/api/Courses/";

axios.interceptors.request.use((config) => {

    const token = window.localStorage.getItem('accessToken');
    if (token) {
        config.headers = {
            ...config.headers,
            Authorization: `Bearer ${token}`,
            "Access-Control-Allow-Origin": "*"
        };
    }
    return config;
}, error => {
    return Promise.reject(error);
})

axios.interceptors.response.use((response) => {

    return response;
}, async (error) => {

    if (error.message === 'Network Error') {

        var model = {
            token: window.localStorage.getItem('refreshToken')
        }

        authService.refreshAccessToken(model)
            .then((response) => {

                const { dispatch } = store;

                dispatch(updateAccess(response.data.accessToken));
            },
                err => {

                    const { dispatch } = store;

                    var model = {
                        type: alertTypes.WARNING,
                        message: "The session was ended. Log in again !"
                    }

                    dispatch(logOut());
                    dispatch(setAlert(model));
                    dispatch(finishLoading());
                })
            .catch(err => {
            });
    }

    return Promise.reject(error);
});

export default class coursesService {

    static getCoursesOnPage(model) {
        return axios.post(URL + 'GetCoursesOnPage', model);
    }

    static getCourses(model) {
        return axios.post(URL + 'GetCoursesOnPageAdmin', model);
    }

    static enroll(model) {
        return axios.post(URL + 'EnrollInACourse', model);
    }

    static getSubscriptions(model) {
        return axios.post(URL + 'GetUserSubscriptionsOnPage', model);
    }

    static unsubscribe(model) {
        return axios.post(URL + 'UnsubscribeFromCourse', model);
    }

    static editCourse(model) {
        return axios.put(URL + 'EditCourse', model);
    }

    static addCourse(model) {
        return axios.post(URL + 'AddCourse', model);
    }

    static removeCourse(model) {
        return axios.post(URL + 'DeleteCourse', model);
    }
}