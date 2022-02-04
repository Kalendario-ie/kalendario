import {createEntityAdapter, createSelector, createSlice} from '@reduxjs/toolkit';
import {adminServiceClient} from 'src/app/api/adminServicesApi';
import {ServiceAdminResourceModel} from 'src/app/api/api';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {compareByName} from 'src/app/shared/util/comparers';
import {BaseSelectors, useInitializeStore} from 'src/app/store/admin/common/adapter';
import {serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';
import {RootState} from 'src/app/store/store';

const storeName = 'adminServices';

const baseSelector = (state: RootState) => state.adminServices;

const adapter = createEntityAdapter<ServiceAdminResourceModel>({
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
export const serviceSelectors: BaseSelectors<ServiceAdminResourceModel> = {
    ...adapterSelectors,
    selectIsInitialized: createSelector(baseSelector, store => store.isInitialized),
    selectByIds: createSelector(
        adapterSelectors.selectEntities,
        (state: any, ids: number[] | string[]) => ids,
        (entities, ids: number[] | string[]) => ids.map(id => entities[id]!).filter(service => !!service)
    ),
};

export const selectServicesWithCategories = createSelector(
    serviceSelectors.selectAll,
    serviceCategorySelectors.selectAll,
    (services, categories) => categories.map(cat =>
        ({...cat, children: services.filter(s => s.serviceCategoryId === cat.id)})
    )
)

export const selectServicesWithCategoriesByIds = createSelector(
    serviceSelectors.selectAll,
    serviceCategorySelectors.selectByIds,
    (services, categories) => categories.map(cat =>
        ({...cat, children: services.filter(s => s.serviceCategoryId === cat.id)})
    )
)

export const serviceReducer = slice.reducer
export const serviceActions = slice.actions


export function useInitializeServices(params: BaseQueryParams = {
    search: undefined,
    start: undefined,
    length: undefined
}) {
    return useInitializeStore(adminServiceClient, serviceSelectors, params, serviceActions);
}
