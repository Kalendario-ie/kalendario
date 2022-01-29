import {adminScheduleClient} from 'src/app/api/adminSchedulesApi';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';

const storeName = 'adminSchedules';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors,
    slice
} = kCreateBaseStore(storeName, adminScheduleClient, (state) => state.adminSchedules);

export {reducer as scheduleReducer}
export {actions as scheduleActions}
export {adapter as scheduleAdapter}
export {selectors as scheduleSelectors}
export {slice as scheduleSlice}
export {sagas as adminScheduleSaga}

