import {createEntityAdapter, createSelector, createSlice} from '@reduxjs/toolkit';
import {adminScheduleClient} from 'src/app/api/adminSchedulesApi';
import {ScheduleAdminResourceModel, ServiceCategoryAdminResourceModel} from 'src/app/api/api';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {RootState} from 'src/app/store/store';

const storeName = 'adminSchedules';

const baseSelector = (state: RootState) => state.adminSchedules;

const adapter = createEntityAdapter<ScheduleAdminResourceModel>({
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
export const scheduleSelectors: BaseSelectors<ScheduleAdminResourceModel> = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};
export const scheduleReducer = slice.reducer
export const scheduleActions = slice.actions

export function useInitializeSchedules(params: BaseQueryParams = {
    search: undefined,
    start: undefined,
    length: undefined
}) {
    return useInitializeStore(adminScheduleClient, scheduleSelectors, params, scheduleActions);
}

