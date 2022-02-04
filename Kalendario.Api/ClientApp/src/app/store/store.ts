import {configureStore} from '@reduxjs/toolkit'
import {TypedUseSelectorHook, useDispatch, useSelector} from 'react-redux';
import {appointmentReducer} from 'src/app/store/admin/appointments';
import {customerReducer} from 'src/app/store/admin/customers';
import {adminDashboardReducer} from 'src/app/store/admin/dashboard';
import {employeeReducer} from 'src/app/store/admin/employees';
import {permissionGroupReducer} from 'src/app/store/admin/permissionGroups';
import {permissionReducer} from 'src/app/store/admin/permissions';
import {scheduleReducer} from 'src/app/store/admin/schedules';
import {serviceCategoryReducer} from 'src/app/store/admin/serviceCategories';
import {userReducer} from 'src/app/store/admin/users';
import {authReducer} from 'src/app/store/auth';
import {companiesReducer} from 'src/app/store/companies';
import {panelReducer} from 'src/app/store/admin/panels';
import {uiReducer} from 'src/app/store/ui';
import {usersReducer} from 'src/app/store/users';
import {serviceReducer} from './admin/services';


export const store = configureStore({
    reducer: {
        auth: authReducer,
        users: usersReducer,
        ui: uiReducer,
        companies: companiesReducer,
        adminDashboard: adminDashboardReducer,
        adminAppointments: appointmentReducer,
        adminSchedulingPanels: panelReducer,
        adminServices: serviceReducer,
        adminServiceCategories: serviceCategoryReducer,
        adminEmployees: employeeReducer,
        adminCustomers: customerReducer,
        adminSchedules: scheduleReducer,
        adminUsers: userReducer,
        adminPermissionGroups: permissionGroupReducer,
        adminPermissions: permissionReducer
    }
})


export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

