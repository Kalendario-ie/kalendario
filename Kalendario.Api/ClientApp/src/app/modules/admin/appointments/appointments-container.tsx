import React from 'react';
import {useInitializeEffect} from 'src/app/shared/admin/hooks';
import {KFlexColumn} from 'src/app/shared/components/flex';
import {employeeActions} from 'src/app/store/admin/employees';
import {scheduleActions} from 'src/app/store/admin/schedules';
import {useReloadAppointmentsEffect} from './employee-panel';


const AppointmentsContainer: React.FunctionComponent = () => {
    useInitializeEffect(employeeActions);
    useInitializeEffect(scheduleActions);
    useReloadAppointmentsEffect();
    // const [openModal, formModal] = useEditModal<Appointment, Appointment>(appointmentSelectors, appointmentActions, AppointmentUpsertForm);
    //TODO: FIX HERE.
    return (
        <KFlexColumn>
            {/*{formModal}*/}
            {/*<KFlexColumn className="sticky-top bg-white-gray">*/}
            {/*    <KFlexRow>*/}
            {/*        <SchedulingPanelsSelector/>*/}
            {/*    </KFlexRow>*/}
            {/*    <KFlexRow>*/}
            {/*        <SchedulingDateSelectorContainer/>*/}
            {/*    </KFlexRow>*/}
            {/*    <EmployeePanelHeadersContainer onCreateClick={openModal}/>*/}
            {/*</KFlexColumn>*/}
            {/*<TimeLineContainer/>*/}
            {/*<EmployeePanelsBodyContainer onSelect={openModal}/>*/}
        </KFlexColumn>
    )
}


export default AppointmentsContainer;
