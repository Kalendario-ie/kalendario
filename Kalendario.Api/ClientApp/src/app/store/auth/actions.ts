import {createAction} from '@reduxjs/toolkit';
import {AuthUser, LoginRequest, RegisterRequest} from 'src/app/api/auth';
import {action} from 'typesafe-actions';
import {ApiBaseError} from '../../api/common/api-errors';
import {ACTION_TYPES} from './types';

export const loginRequest = (request: LoginRequest) =>
    action(ACTION_TYPES.LOGIN_REQUEST, request);

export const loginRequestSuccess = () =>
    action(ACTION_TYPES.LOGIN_REQUEST_SUCCESS)

export const loginRequestFail = (error?: ApiBaseError) =>
    action(ACTION_TYPES.LOGIN_REQUEST_FAIL, error)

export const registerRequest = (request: RegisterRequest) =>
    action(ACTION_TYPES.REGISTER_REQUEST, request);

export const registerRequestSuccess = () =>
    action(ACTION_TYPES.REGISTER_REQUEST_SUCCESS)

export const registerRequestFail = (error?: ApiBaseError) =>
    action(ACTION_TYPES.REGISTER_REQUEST_FAIL, error)

export const setUser = (user: AuthUser | null) =>
    action(ACTION_TYPES.SET_USER, user)

export const setLoadingUser = createAction<boolean>(ACTION_TYPES.SET_LOADING_USER);

export const setIsSubmitting = createAction<boolean>(ACTION_TYPES.SET_IS_SUBMITTING);

export const facebookLoginRequest = (accessToken: string) =>
    action(ACTION_TYPES.FACEBOOK_LOGIN_REQUEST, accessToken);

export const facebookLoginRequestSuccess = () =>
    action(ACTION_TYPES.FACEBOOK_LOGIN_REQUEST_SUCCESS)

export const facebookLoginRequestFail = (error?: ApiBaseError) =>
    action(ACTION_TYPES.FACEBOOK_LOGIN_REQUEST_FAIL, error)
