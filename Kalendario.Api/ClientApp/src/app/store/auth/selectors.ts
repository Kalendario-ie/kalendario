import {Profile} from 'oidc-client';
import {RootState} from 'src/app/store/store';


export const selectLoggedIn: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loggedIn;

export const selectUser: (rootState: RootState) => Profile | null =
    (rootState) => rootState.auth.user

export const selectLoadingUser: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loadingUser;
