import {adminPermissionGroupClient} from 'src/app/api/adminRoleGroupsApi';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';

const storeName = 'adminPermissionGroups';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors,
    slice
} = kCreateBaseStore(storeName, adminPermissionGroupClient, (state) => state.adminPermissionGroups);


export {reducer as permissionGroupReducer}
export {actions as permissionGroupActions}
export {adapter as permissionGroupAdapter}
export {selectors as permissionGroupSelectors}
export {slice as permissionGroupSlice}
export {sagas as adminPermissionGroupSaga}

