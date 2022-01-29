import {ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand} from 'src/app/api/api';
import {adminServiceCategoryClient} from 'src/app/api/adminServiceCategoryApi';
import {BaseQueryParams} from 'src/app/api/common/clients/base-django-api';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';

const storeName = 'adminServiceCategories';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors,
    slice,
} = kCreateBaseStore<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand, BaseQueryParams>(storeName, adminServiceCategoryClient, (state) => state.adminServiceCategories);

export {reducer as serviceCategoryReducer}
export {actions as serviceCategoryActions}
export {adapter as serviceCategoryAdapter}
export {selectors as serviceCategorySelectors}
export {slice as serviceCategorySlice}
export {sagas as adminServiceCategorySaga}

