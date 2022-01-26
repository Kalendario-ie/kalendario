import {createAction, createSelector, createSlice} from '@reduxjs/toolkit';
import {call, put, select, takeEvery} from 'redux-saga/effects';
import {adminPermissionGroupClient} from 'src/app/api/adminRoleGroupsApi';
import {RoleAdminResourceModel} from 'src/app/api/api';
import {ApiBaseError} from 'src/app/api/common/api-errors';
import {RootState} from 'src/app/store/store';
import {PayloadAction} from 'typesafe-actions';


interface PermissionsState {
    initialized: boolean;
    permissions: RoleAdminResourceModel[];
    apiError: ApiBaseError | null;
}

const initialState: PermissionsState = {
    initialized: false,
    permissions: [],
    apiError: null
}

const permissionSlice = createSlice({
    name: 'adminPermissions',
    initialState,
    reducers: {
        setPermissions(state, action: PayloadAction<string, RoleAdminResourceModel[]>) {
            state.permissions = action.payload;
            state.initialized = true;
        },
        setApiError(state, action: PayloadAction<string, ApiBaseError>) {
            state.apiError = action.payload;
            state.initialized = false;
        }
    }
})

const {reducer, actions, name} = permissionSlice;

const baseSelector: (rootState: RootState) => PermissionsState =
    (rootState) => rootState.adminPermissions;

const selectors = {
    selectAll: createSelector(
        baseSelector,
        state => state.permissions
    ),
    selectIsInitialized: createSelector(
        baseSelector,
        state => state.initialized
    )
}

export const permissionsActions = {
    initializeStore: createAction(`${name}/initializeStore`)
}

function* initializeStore(action: { type: string, payload: string }) {
    const isInitialized: boolean = yield select(selectors.selectIsInitialized);
    if (!isInitialized) {
        try {
            const permissions: RoleAdminResourceModel[] = yield call(adminPermissionGroupClient.roles);
            yield put(actions.setPermissions(permissions));
        } catch (error) {
            yield put(actions.setApiError(error as ApiBaseError));
        }
    }
}

export function* adminPermissionsSagas() {
    yield takeEvery(permissionsActions.initializeStore.type, initializeStore);
}

export {reducer as permissionReducer}
export {selectors as permissionSelectors}
