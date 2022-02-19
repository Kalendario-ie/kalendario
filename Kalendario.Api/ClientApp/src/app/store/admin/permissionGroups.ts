import {createEntityAdapter, createSelector, createSlice, OutputSelector} from '@reduxjs/toolkit';
import {adminPermissionGroupClient} from 'src/app/api/adminRoleGroupsApi';
import {RoleGroupAdminResourceModel} from 'src/app/api/api';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {RootState} from 'src/app/store/store';

const storeName = 'adminPermissionGroups';

const baseSelector = (state: RootState) => state.adminPermissionGroups;

const adapter = createEntityAdapter<RoleGroupAdminResourceModel>({
    selectId: (entity) => entity.id,
    sortComparer: compareByName,
})

export const slice = createSlice({
    name: storeName,
    initialState: adapter.getInitialState({
        isInitialized: false
    }),
    reducers: {
        upsertMany: adapter.upsertMany,
        setAll: adapter.setAll,
        upsertOne: adapter.upsertOne,
        removeOne: adapter.removeOne,
        removeAll: adapter.removeAll,
        setInitialized: (state, action) => {
            state.isInitialized = action.payload
        }
    }
});

const adapterSelectors = adapter.getSelectors<RootState>(baseSelector);
export const permissionGroupSelectors: BaseSelectors<RoleGroupAdminResourceModel> = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};
export const permissionGroupReducer = slice.reducer
export const permissionGroupActions = slice.actions

const selectIsInitialized: OutputSelector<RootState, boolean, any> = createSelector(baseSelector, store => store.isInitialized);

export function useInitializePermissionGroups(params: BaseQueryParams = {
    search: undefined,
    start: undefined,
    length: undefined
}) {
    return useInitializeStore(adminPermissionGroupClient, permissionGroupSelectors, params, permissionGroupActions);
}

