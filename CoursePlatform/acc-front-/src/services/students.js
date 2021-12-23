import axios from "axios";

const URL = "https://localhost:5001/api/Users/";

axios.interceptors.request.use((config) => {
    const token = window.localStorage.getItem('accessToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
})

export default class usersService {

    static getStudents() {
        return axios.get(URL + 'GetStudents');
    }

    static editUser(model) {
        return axios.put(URL + 'EditUser', model);
    }

    static removeUser(model) {
        return axios.post(URL + 'DeleteUser', model);
    }
}