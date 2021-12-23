import axios from "axios";

const URL = "https://localhost:5001/api/Auth/";

export default class authService {

    static logIn(model) {
        return axios.post(URL + 'LogIn', model);
    }

    static LogInViaFacebook(model) {
        return axios.post(URL +'LogInViaFacebook', model);
    }
    
    static registerUser(model) {
        return axios.post(URL + 'RegisterUser', model);
    }

    static confirmEmail(model) {
        return axios.post(URL + 'ConfirmEmail', model);
    }

    static refreshAccessToken(model) {
        return axios.post(URL +'RefreshAccessToken', model);
    }
}