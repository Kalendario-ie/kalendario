import React from 'react';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import SchedulingDateSelectorContainer
    from 'src/app/modules/admin/appointments/date-selector/scheduling-date-selector-container';
import TimeLineContainer from 'src/app/modules/admin/appointments/employee-panel/TimeLineContainer';
import AppointmentUpsertForm from 'src/app/modules/admin/appointments/forms/appointment-upsert-form';
import TimeLockUpsertForm from 'src/app/modules/admin/appointments/forms/time-lock-upsert-form';
import SchedulingPanelsSelector from 'src/app/modules/admin/appointments/scheduling-panels/scheduling-panels-selector';
import {useEditModal, useInitializeEffect} from 'src/app/shared/admin/hooks';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {adminAppointmentSlice, appointmentSelectors} from 'src/app/store/admin/appointments';
import {employeeActions} from 'src/app/store/admin/employees';
import {scheduleActions} from 'src/app/store/admin/schedules';
import {
    EmployeePanelHeadersContainer,
    EmployeePanelsBodyContainer,
    useReloadAppointmentsEffect
} from './employee-panel';


const AppointmentsContainer: React.FunctionComponent = () => {
    useInitializeEffect(employeeActions);
    useInitializeEffect(scheduleActions);
    useReloadAppointmentsEffect();

    const [openAppointmentUpsertForm, formModal] =
        useEditModal(appointmentSelectors, adminAppointmentSlice.actions, AppointmentUpsertForm);
    const [openTimeLockUpsertForm, timeLockFormModal] =
        useEditModal(appointmentSelectors, adminAppointmentSlice.actions, TimeLockUpsertForm);

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
