import axios from "axios";

const URL = "http://localhost:5000/api/Courses/";

axios.interceptors.request.use((config) => {
    const token = window.localStorage.getItem('authToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
})

export default class coursesService {

    static getAllCourses(model) {
        return axios.post(URL + 'GetCourses', model);
    }

    static enroll(model) {
        return axios.post(URL + 'EnrollInACourse', model);
    }

    static getSubscriptions(model) {
        return axios.post(URL + 'GetUserCourses', model);
    }

    static unsubscribe(model) {
        return axios.post(URL + 'UnsubscribeFromCourse', model);
    }

    static editCourse(model) {
        return axios.put(URL + 'EditCourse', model);
    }

}