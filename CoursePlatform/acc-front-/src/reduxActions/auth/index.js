import * as types from "./types";

export const changeAuthPage = (model) => {
    return {
        type: types.CHANGE_AUTH_PAGE,
        payload: model
    };
}