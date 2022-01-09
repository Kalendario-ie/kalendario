import {createAction} from '@reduxjs/toolkit';
import {Profile} from 'oidc-client';
import {action} from 'typesafe-actions';
import {ACTION_TYPES} from './types';

export const setUser = (user: Profile | null) => action(ACTION_TYPES.SET_USER, user)

export const setLoadingUser = createAction<boolean>(ACTION_TYPES.SET_LOADING_USER);

