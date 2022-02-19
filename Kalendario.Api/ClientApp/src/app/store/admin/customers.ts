import {createEntityAdapter, createSelector, createSlice} from '@reduxjs/toolkit';
import {adminCustomerClient} from 'src/app/api/adminCustomerApi';
import {CustomerAdminResourceModel} from 'src/app/api/api';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {RootState} from 'src/app/store/store';

const storeName = 'adminCustomers';
const baseSelector = (state: RootState) => state.adminCustomers;

const adapter = createEntityAdapter<CustomerAdminResourceModel>({
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
export const customerSelectors: BaseSelectors<CustomerAdminResourceModel> = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};
export const customerReducer = slice.reducer
export const customerActions = slice.actions


export function useInitializeCustomers(params: BaseQueryParams) {
    return useInitializeStore(adminCustomerClient, customerSelectors, params, customerActions);
}

