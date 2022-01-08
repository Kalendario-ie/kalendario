import {createSelector} from '@reduxjs/toolkit';
import {AuthUser} from 'src/app/api/auth';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {employeeParser} from 'src/app/api/employees';
import {RootState} from 'src/app/store/store';


export const selectLoggedIn: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loggedIn;

export const selectApiError: (rootState: RootState) => ApiValidationError | null =
    (rootState) => rootState.auth.apiError;

export const selectUser: (rootState: RootState) => AuthUser | null =
    (rootState) => rootState.auth.user

export const selectUserEmployee = createSelector(selectUser, user => employeeParser(null)) // TODO: Fix here.

export const selectLoadingUser: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.loadingUser;

export const selectIsSubmitting: (rootState: RootState) => boolean =
    (rootState) => rootState.auth.isSubmitting;
