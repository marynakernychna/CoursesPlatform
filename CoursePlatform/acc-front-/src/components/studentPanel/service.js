import axios from "axios";

const URL_Courses = "http://localhost:5000/api/Courses/";
const URL_Users = "http://localhost:5000/api/Users/";

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

    static getCoursesList(model) {
        return axios.post(URL_Courses + 'GetCourses', model);
    }

    static enrollInACourse(model) {
        return axios.post(URL_Courses + 'EnrollInACourse', model);
    }

    static getUserCourses(model) {
        return axios.post(URL_Courses + 'GetUserCourses', model);
    }

    static unsubscribe(model) {
        return axios.post(URL_Courses + 'UnsubscribeFromCourse', model);
    }

    static removeCourse(model) {
        return axios.post(URL_Courses + 'DeleteCourse', model);
    }

    static editCourse(model) {
        return axios.put(URL_Courses + 'EditCourse', model);
    }

    static addCourse(model) {
        return axios.post(URL_Courses + 'AddCourse', model);
    }

    static getStudents(model) {
        return axios.post(URL_Users + 'GetUsers', model);
    }

    static removeStudent(model) {
        return axios.post(URL_Users + 'DeleteUser', model);
    }

    static editStudent(model) {
        return axios.put(URL_Users + 'EditUser', model);
    }
}