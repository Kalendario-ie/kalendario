import {Duration, Moment} from 'moment';
import {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {adminAppointmentClient, AppointmentsGetParams} from 'src/app/api/adminAppointments';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {appointmentActions} from 'src/app/store/admin/appointments';
import {adminDashboardSelectors} from 'src/app/store/admin/dashboard';
import {employeeSelectors} from 'src/app/store/admin/employees';


export function useSelectPanelEmployees() {
    const selectedPanel = useAppSelector(adminDashboardSelectors.selectSelectedPanel)
    return useAppSelector(state => employeeSelectors.selectByIds(state, selectedPanel?.employeeIds || []));
}


export function useReloadAppointmentsEffect() {
    const selectedPanel = useAppSelector(adminDashboardSelectors.selectSelectedPanel)
    const currentDate = useSelector(adminDashboardSelectors.selectCurrentDate);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(appointmentActions.setIsLoading(true));

        const query: AppointmentsGetParams = {
            fromDate: currentDate.toISOString(),
            toDate: currentDate.clone().add(1, 'day').toISOString(),
            employeeIds: selectedPanel?.employeeIds || [],
            customerId: undefined,
        };

        adminAppointmentClient.get(query)
            .then(result => {
                dispatch(appointmentActions.setAll(result.entities || []));
                dispatch(appointmentActions.setIsLoading(false));
            });

    }, [selectedPanel, currentDate, dispatch]);

}


export function useHoursConverter(value: Moment | Duration): string {
    const slotSize = useAppSelector(adminDashboardSelectors.selectSlotSize);
    return `${(value.hours() + (value.minutes() / 60)) * slotSize}rem`;
}
