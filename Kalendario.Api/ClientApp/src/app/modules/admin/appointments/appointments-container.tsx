import React from 'react';
import {adminAppointmentClient, appointmentClient} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel, UpsertAppointmentCommand, UpsertTimeLockCommand} from 'src/app/api/api';
import SchedulingDateSelectorContainer
    from 'src/app/modules/admin/appointments/date-selector/scheduling-date-selector-container';
import TimeLineContainer from 'src/app/modules/admin/appointments/employee-panel/TimeLineContainer';
import AppointmentUpsertForm from 'src/app/modules/admin/appointments/forms/appointment-upsert-form';
import TimeLockUpsertForm from 'src/app/modules/admin/appointments/forms/time-lock-upsert-form';
import SchedulingPanelsSelector from 'src/app/modules/admin/appointments/scheduling-panels/scheduling-panels-selector';
import {useEditModal, useEditModal2, useInitializeEffect} from 'src/app/shared/admin/hooks';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {useAppDispatch} from 'src/app/store';
import {
    adminAppointmentSlice,
    appointmentActions,
    appointmentAdapter,
    appointmentSelectors
} from 'src/app/store/admin/appointments';
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
    const dispatch = useAppDispatch();

    const handleSubmit = (command: UpsertTimeLockCommand, id: string | undefined): void => {
        if (!id) {
            adminAppointmentClient
                .createTimeLock(command)
                .then(res => dispatch(adminAppointmentSlice.actions.upsertOne(res)));
        } else {
            adminAppointmentClient
                .updateTimeLock(id, command)
                .then(res => dispatch(adminAppointmentSlice.actions.upsertOne(res)))
        }
    }

    const [openModal, formModal] = useEditModal<AppointmentAdminResourceModel, UpsertAppointmentCommand>(appointmentSelectors, appointmentActions, AppointmentUpsertForm);
    const [openTimeLockModal, timeLockFormModal] = useEditModal2<AppointmentAdminResourceModel, UpsertTimeLockCommand>(appointmentSelectors, appointmentActions, TimeLockUpsertForm, handleSubmit);

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
                <EmployeePanelHeadersContainer onCreateClick={openModal} onCreateLockClick={openTimeLockModal}/>
            </KFlexColumn>
            <TimeLineContainer/>
            <EmployeePanelsBodyContainer onSelect={openModal} onLockClick={openTimeLockModal}/>
        </KFlexColumn>
    )
}


export default AppointmentsContainer;
