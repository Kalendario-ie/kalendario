import React, {useEffect, useState} from 'react';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import KModal from 'src/app/shared/components/modal/k-modal';
import {KCard} from 'src/app/shared/components/primitives/containers';
import KIcon from 'src/app/shared/components/primitives/k-icon';
import {useTimeFormatter} from 'src/app/shared/util/time-formater';

interface AppointmentHistoryItemProps {
    // appointment: AppointmentHistory; // TODO: This should be history
}

const AppointmentHistoryItem: React.FunctionComponent<AppointmentHistoryItemProps> = (
    {
        // appointment
    }) => {
    const timeFormatter = useTimeFormatter();
    // const historyDate = appointment.historyDate ? timeFormatter(appointment.historyDate) : null;

    return (
        <KCard hasShadow={false}
               className="my-2 small"
               bodiless={true}
               footer={
                   <KFlexRow className="small" justify="between">
                       {/*<KFlexColumn>{appointment.historyUser?.name}</KFlexColumn> // TODO: FIX HERE*/}
                       {/*<KFlexColumn>{historyDate}</KFlexColumn>*/}
                   </KFlexRow>
               }
        >
            {/*<KFlexRow align="center" justify="center">*/}
            {/*    {timeFormatter(appointment.start)}*/}
            {/*    <KIcon icon="clock" color="primary" margin={2}/>*/}
            {/*    {timeFormatter(appointment.end)}*/}
            {/*</KFlexRow>*/}
            {/*<KFlexRow align="center">*/}
            {/*    <KIcon icon="user" color="primary" margin={2}/> {appointment.employee.name}*/}
            {/*</KFlexRow>*/}
            {/*{appointment.service &&*/}
            {/*<KFlexRow align="center">*/}
            {/*    <KIcon icon="magic" color="primary" margin={2}/> {appointment.service} // TODO SERVICE.NAME*/}
            {/*</KFlexRow>*/}
            {/*}*/}
            {/*{appointment.customer &&*/}
            {/*<KFlexRow align="center">*/}
            {/*    <KIcon icon="address-card" color="primary" margin={2}/> {appointment.customer} //todo: customer.name*/}
            {/*</KFlexRow>*/}
            {/*}*/}
            {/*{appointment.internalNotes &&*/}
            {/*<KFlexRow align="center" className="mb-2">*/}
            {/*    <KIcon icon="sticky-note" color="primary" margin={2}/> {appointment.internalNotes}*/}
            {/*</KFlexRow>*/}
            {/*}*/}
            {/*{appointment.customerNotes &&*/}
            {/*<KFlexRow align="center" className="mb-2">*/}
            {/*    <KIcon icon="comment-alt" color="primary" margin={2}/> {appointment.customerNotes}*/}
            {/*</KFlexRow>*/}
            {/*}*/}
        </KCard>
    )
}

interface AppointmentHistoryContainerProps {
    id?: number | string; // TODO: THIS IS STRING ONLY
    isOpen: boolean;
    onClose: () => void;
}

const AppointmentHistoryContainer: React.FunctionComponent<AppointmentHistoryContainerProps> = (
    {
        id,
        isOpen,
        onClose
    }) => {
    // const [appointments, setAppointments] = useState<AppointmentAdminResourceModel[]>([]);

    useEffect(() => {
        // adminAppointmentClient.history(id)
        //     .then(res => {
        //         setAppointments(res.results);
        //     }); // todo: fix here.
    }, [id]);

    return (
        <KModal body={
            <KFlexColumn>
                {/*{appointments.map(appointment => <AppointmentHistoryItem key={appointment.id}*/}
                {/*                                                         appointment={appointment}/>*/}
                {/*)}*/}
            </KFlexColumn>
        }
                header="history"
                onCancel={onClose}
                isOpen={isOpen}>

        </KModal>
    )
}


export default AppointmentHistoryContainer;
