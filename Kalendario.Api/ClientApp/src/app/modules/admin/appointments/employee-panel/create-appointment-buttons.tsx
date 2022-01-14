import {Moment} from 'moment';
import React from 'react';
import {AppointmentAdminResourceModel, EmployeeAdminResourceModel} from 'src/app/api/api';

interface CreateAppointmentButtonsProps {
    employee: EmployeeAdminResourceModel;
    onCreateClick: (entity: AppointmentAdminResourceModel | null) => () => void;
    currentDate: Moment;
    hour: number;
    minute: number;
}

const CreateAppointmentButtons: React.FunctionComponent<CreateAppointmentButtonsProps> = (
    {
        employee,
        onCreateClick,
        currentDate,
        hour,
        minute
    }) => {
    const employeeId = employee.id;
    const selectedTime = () => currentDate.clone().add(hour, 'hour').add(minute, 'minute');
    // const handleAddClick = () =>
    //     onCreateClick(blankCustomerAppointment(employeeId, selectedTime()))();
    // const handleLockClick = () =>
    //     onCreateClick(blankEmployeeEvent(employeeId, selectedTime().add(minute, 'minute')))();

    return (
        <>
            {/*<KIconButton color="primary" icon={'plus'} onClick={handleAddClick}/>*/}
            {/*<KIconButton color="accent" icon={'lock'} onClick={handleLockClick}/>*/}
        </>
    )
}


export default CreateAppointmentButtons;
