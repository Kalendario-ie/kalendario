import {createEntityAdapter, createSelector, createSlice, OutputSelector} from '@reduxjs/toolkit';
import {adminSchedulingPanelsClient} from 'src/app/api/adminSchedulingPanels';
import {
    EmployeeAdminResourceModel,
    ScheduleAdminResourceModel,
    SchedulingPanelAdminResourceModel
} from 'src/app/api/api';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {RootState} from 'src/app/store/store';

const storeName = 'adminSchedulingPanels';

const baseSelector = (state: RootState) => state.adminSchedulingPanels;

const adapter = createEntityAdapter<SchedulingPanelAdminResourceModel>({
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
export const panelSelectors: BaseSelectors<SchedulingPanelAdminResourceModel> = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};
export const panelReducer = slice.reducer
export const panelActions = slice.actions

export function useInitializePanels(params: BaseQueryParams = {
    search: undefined,
    start: undefined,
    length: undefined
}) {
    return useInitializeStore(adminSchedulingPanelsClient, panelSelectors, params, panelActions);
}
