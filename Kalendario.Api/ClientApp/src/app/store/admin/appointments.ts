import {createEntityAdapter, createSelector, createSlice} from '@reduxjs/toolkit';
import {adminAppointmentClient, AppointmentsGetParams} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {RootState} from 'src/app/store/store';

const storeName = 'adminAppointments';


const baseSelector = (state: RootState) => state.adminAppointments;

const adapter = createEntityAdapter<AppointmentAdminResourceModel>({
    selectId: (entity) => entity.id,
    sortComparer: compareByName,
})

export const slice = createSlice({
    name: storeName,
    initialState: adapter.getInitialState({
        isInitialized: false,
        isLoading: false,
    }),
    reducers: {
        upsertMany: adapter.upsertMany,
        setAll: adapter.setAll,
        upsertOne: adapter.upsertOne,
        removeOne: adapter.removeOne,
        removeAll: adapter.removeAll,
        setInitialized: (state, action) => {
            state.isInitialized = action.payload
        },
        setIsLoading: (state, action) => {
            state.isLoading = action.payload
        }
    }
});

interface AppointmentSelectors extends BaseSelectors<AppointmentAdminResourceModel> {
    selectIsLoading: (state: RootState) => boolean;
}

const adapterSelectors = adapter.getSelectors<RootState>(baseSelector);
export const appointmentSelectors: AppointmentSelectors = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectIsLoading: createSelector(baseSelector, store => store.isLoading),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};
export const appointmentReducer = slice.reducer
export const appointmentActions = slice.actions

export function useInitializeAppointments(params: AppointmentsGetParams) {
    return useInitializeStore(adminAppointmentClient, appointmentSelectors, params, appointmentActions);
}

