import axios from "axios";

const URL = "http://localhost:5000/api/Auth/";

export default class c {

    static test(model) {
        return axios.post(URL + 'ConfirmEmail', model);
    }
}