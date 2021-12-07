import axios from "axios";

const URL = "http://localhost:5000/api/Auth/";

export default class authService {
    static logIn(model) {
        return axios.post(URL + 'LogIn', model);
    }

    static registerUser(model) {
        return axios.post(URL + 'RegisterUser', model);
    }
}