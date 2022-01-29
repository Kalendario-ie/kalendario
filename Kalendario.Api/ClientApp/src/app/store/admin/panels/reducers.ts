import {adminSchedulingPanelsClient} from 'src/app/api/adminSchedulingPanels';
import {kCreateBaseStore} from 'src/app/store/admin/common/adapter';

const storeName = 'adminSchedulingPanels';

const {
    actions,
    adapter,
    reducer,
    sagas,
    selectors,
    slice
} = kCreateBaseStore(storeName, adminSchedulingPanelsClient, (state) => state.adminSchedulingPanels);

export {reducer as schedulingPanelReducer}
export {actions as schedulingPanelActions}
export {adapter as schedulingPanelAdapter}
export {selectors as schedulingPanelSelectors}
export {slice as schedulingPanelSlice}
export {sagas as adminSchedulingPanelSaga}

