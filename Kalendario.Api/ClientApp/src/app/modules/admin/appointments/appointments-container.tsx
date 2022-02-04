import React from 'react';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import SchedulingDateSelectorContainer
    from 'src/app/modules/admin/appointments/date-selector/scheduling-date-selector-container';
import TimeLineContainer from 'src/app/modules/admin/appointments/employee-panel/TimeLineContainer';
import AppointmentUpsertForm from 'src/app/modules/admin/appointments/forms/appointment-upsert-form';
import TimeLockUpsertForm from 'src/app/modules/admin/appointments/forms/time-lock-upsert-form';
import SchedulingPanelsSelector from 'src/app/modules/admin/appointments/scheduling-panels/scheduling-panels-selector';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {appointmentActions} from 'src/app/store/admin/appointments';
import {useInitializeEmployees} from 'src/app/store/admin/employees';
import {useInitializeSchedules} from 'src/app/store/admin/schedules';
import {
    EmployeePanelHeadersContainer,
    EmployeePanelsBodyContainer,
    useReloadAppointmentsEffect
} from './employee-panel';


const AppointmentsContainer: React.FunctionComponent = () => {
    useInitializeEmployees();
    useInitializeSchedules();
    useReloadAppointmentsEffect();

    const [openAppointmentUpsertForm, formModal] = useEditModal(appointmentActions, AppointmentUpsertForm);
    const [openTimeLockUpsertForm, timeLockFormModal] = useEditModal(appointmentActions, TimeLockUpsertForm);

    const onSelect = (appointment: AppointmentAdminResourceModel) =>
        (appointment.serviceId != null) ? openAppointmentUpsertForm(appointment) : openTimeLockUpsertForm(appointment);


    return (
        <KFlexColumn>
            {formModal}
            {timeLockFormModal}
            <KFlexColumn className="sticky-top bg-white-gray">
                <KFlexRow>
                    <SchedulingPanelsSelector/>
                </KFlexRow>
                <KFlexRow>
                    <SchedulingDateSelectorContainer/>
                </KFlexRow>
                <EmployeePanelHeadersContainer onCreateClick={openAppointmentUpsertForm}
                                               onCreateLockClick={openTimeLockUpsertForm}/>
            </KFlexColumn>
            <TimeLineContainer/>
            <EmployeePanelsBodyContainer onSelect={onSelect} onCreateClick={openAppointmentUpsertForm}
                                         onLockClick={openTimeLockUpsertForm}/>
        </KFlexColumn>
    )
}


export default AppointmentsContainer;
