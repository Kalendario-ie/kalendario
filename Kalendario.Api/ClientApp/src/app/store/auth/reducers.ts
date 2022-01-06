import {Reducer} from 'redux';
import {AuthUser} from 'src/app/api/auth';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {isLoggedIn} from 'src/app/api/common/session-storage';
import {ACTION_TYPES} from './types';

export interface AuthState {
    apiError: ApiValidationError | null;
    loggedIn: boolean;
    loadingUser: boolean;
    user: AuthUser | null;
    isSubmitting: boolean;
}

const initialState: AuthState = {
    apiError: null,
    loggedIn: isLoggedIn(),
    loadingUser: false,
    user: null,
    isSubmitting: false,
}

const reducer: Reducer<AuthState> = (state = initialState, {type, payload}) => {
    switch (type) {
        case ACTION_TYPES.REGISTER_REQUEST:
        case ACTION_TYPES.LOGIN_REQUEST:
            return {...state, apiError: null}
        case ACTION_TYPES.SET_USER:
            return {...state, loggedIn: !!payload, user: payload, loadingUser: false}
        case ACTION_TYPES.SET_LOADING_USER:
            return {...state, loadingUser: payload}
        case ACTION_TYPES.REGISTER_REQUEST_FAIL:
        case ACTION_TYPES.LOGIN_REQUEST_FAIL:
            return {...state, loggedIn: false, apiError: payload, loadingUser: false}
        default:
            return {...state}
    }
}

export {reducer as authReducer};
