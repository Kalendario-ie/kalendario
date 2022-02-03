import {Moment} from 'moment';
import React from 'react';
import {
    blankAppointment,
    upsertAppointmentCommandParser,
    upsertTimeLockCommandParser
} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel, EmployeeAdminResourceModel} from 'src/app/api/api';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';

interface CreateAppointmentButtonsProps {
    employee: EmployeeAdminResourceModel;
    onCreateClick: (entity: AppointmentAdminResourceModel) => void;
    onCreateLockClick: (entity: AppointmentAdminResourceModel) => void;
    currentDate: Moment;
    hour: number;
    minute: number;
}

const CreateAppointmentButtons: React.FunctionComponent<CreateAppointmentButtonsProps> = (
    {
        employee,
        onCreateClick,
        onCreateLockClick,
        currentDate,
        hour,
        minute
    }) => {
    const employeeId = employee.id;
    const selectedTime = () => currentDate.clone().add(hour, 'hour').add(minute, 'minute');
    const handleAddClick = () =>
        onCreateClick(blankAppointment(employeeId, selectedTime().toISOString(), selectedTime().toISOString()));
    const handleLockClick = () =>
        onCreateLockClick(blankAppointment(employeeId, selectedTime().toISOString(), selectedTime().toISOString()));

    return (
        <>
            <KIconButton color="primary" icon={'plus'} onClick={handleAddClick}/>
            <KIconButton color="accent" icon={'lock'} onClick={handleLockClick}/>
        </>
    )
}


export default CreateAppointmentButtons;
