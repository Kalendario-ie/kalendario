import {Moment} from 'moment';
import React from 'react';
import {upsertAppointmentCommandParser} from 'src/app/api/adminAppointments';
import {EmployeeAdminResourceModel, UpsertAppointmentCommand} from 'src/app/api/api';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';

interface CreateAppointmentButtonsProps {
    employee: EmployeeAdminResourceModel;
    onCreateClick: (entity: UpsertAppointmentCommand) => () => void;
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
    const handleAddClick = () => onCreateClick({
        ...upsertAppointmentCommandParser(null)
        , employeeId, start: selectedTime(), end: selectedTime()
    })();
    // const handleLockClick = () =>
    //     onCreateClick(blankEmployeeEvent(employeeId, selectedTime().add(minute, 'minute')))();

    return (
        <>
            <KIconButton color="primary" icon={'plus'} onClick={handleAddClick}/>
            {/*<KIconButton color="accent" icon={'lock'} onClick={handleLockClick}/>*/}
        </>
    )
}


export default CreateAppointmentButtons;
