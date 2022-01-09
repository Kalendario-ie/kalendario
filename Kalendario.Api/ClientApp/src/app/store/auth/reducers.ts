import {Profile} from 'oidc-client';
import {Reducer} from 'redux';
import {isLoggedIn} from 'src/app/api/common/session-storage';
import {ACTION_TYPES} from './types';

export interface AuthState {
    loggedIn: boolean;
    loadingUser: boolean;
    user: Profile | null;
}

const initialState: AuthState = {
    loggedIn: isLoggedIn(),
    loadingUser: false,
    user: null,
}

const reducer: Reducer<AuthState> = (state = initialState, {type, payload}) => {
    switch (type) {
        case ACTION_TYPES.SET_USER:
            return {...state, loggedIn: !!payload, user: payload, loadingUser: false}
        default:
            return {...state}
    }
}

export {reducer as authReducer};
