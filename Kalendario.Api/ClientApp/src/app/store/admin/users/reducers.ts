import {adminUsersClient} from 'src/app/api/adminUsersApi';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';

const storeName = 'adminUsers';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors,
    slice
} = kCreateBaseStore(storeName, adminUsersClient, (state) => state.adminUsers);

export {reducer as userReducer}
export {actions as userActions}
export {adapter as userAdapter}
export {selectors as userSelectors}
export {slice as userSlice}
export {sagas as adminUserSaga}

