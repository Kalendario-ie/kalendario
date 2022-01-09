import {createSelector} from '@reduxjs/toolkit';
import {Profile} from 'oidc-client';
import {employeeParser} from 'src/app/api/employees';
import {RootState} from 'src/app/store/store';


export const selectLoggedIn: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loggedIn;

export const selectUser: (rootState: RootState) => Profile | null =
    (rootState) => rootState.auth.user

export const selectUserEmployee = createSelector(selectUser, user => employeeParser(null)) // TODO: Fix here.

export const selectLoadingUser: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loadingUser;
