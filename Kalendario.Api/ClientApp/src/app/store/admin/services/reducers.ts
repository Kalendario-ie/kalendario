import {createSelector} from '@reduxjs/toolkit';
import {adminServiceClient} from 'src/app/api/services';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';
import {serviceCategorySelectors} from '../serviceCategories';

const storeName = 'adminServices';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors
} = kCreateBaseStore(storeName, adminServiceClient, (state) => state.adminServices);

const selectServicesWithCategories = createSelector(
    selectors.selectAll,
    serviceCategorySelectors.selectAll,
    (services, categories) => categories.map(cat =>
        ({...cat, children: services.filter(s => s.serviceCategoryId === cat.id)})
    )
)

const selectServicesWithCategoriesByIds = createSelector(
    selectors.selectAll,
    serviceCategorySelectors.selectByIds,
    (services, categories) => categories.map(cat =>
        ({...cat, children: services.filter(s => s.serviceCategoryId === cat.id)})
    )
)

export const serviceSelectors = {
    ...selectors,
    selectServicesWithCategories,
    selectServicesWithCategoriesByIds
};

export {reducer as serviceReducer}
export {actions as serviceActions}
export {adapter as serviceAdapter}
export {sagas as adminServiceSaga}

