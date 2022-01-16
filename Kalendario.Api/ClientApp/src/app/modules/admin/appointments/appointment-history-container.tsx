import moment from 'moment';
import React, {useEffect, useState} from 'react';
import {adminAppointmentClient} from 'src/app/api/adminAppointments';
import {AppointmentHistoryAdminResourceModel} from 'src/app/api/api';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import KModal from 'src/app/shared/components/modal/k-modal';
import {KCard} from 'src/app/shared/components/primitives/containers';
import KIcon from 'src/app/shared/components/primitives/k-icon';
import {useTimeFormatter} from 'src/app/shared/util/time-formater';

interface AppointmentHistoryItemProps {
    appointment: AppointmentHistoryAdminResourceModel;
}

const AppointmentHistoryItem: React.FunctionComponent<AppointmentHistoryItemProps> = (
    {
        appointment
    }) => {
    const timeFormatter = useTimeFormatter();
    const historyDate = appointment.date ? timeFormatter(appointment.date) : null;

    return (
        <KCard hasShadow={false}
               className="my-2 small"
               bodiless={true}
               footer={
                   <KFlexRow className="small" justify="between">
                       <KFlexColumn>{appointment.user.userName}</KFlexColumn>
                       <KFlexColumn>{historyDate}</KFlexColumn>
                   </KFlexRow>
               }
        >
            <KFlexRow className="font-weight-bold m-1" align="center">
                {appointment.entityState}
            </KFlexRow>
            {appointment.start && moment(appointment.start).isValid() &&
            <KFlexRow align="center">
                <KIcon icon="hourglass-start" color="primary" margin={2}/> {timeFormatter(appointment.start)}
            </KFlexRow>
            }
            {appointment.end && moment(appointment.end).isValid() &&
                <KFlexRow align="center">
                    <KIcon icon="hourglass-end" color="primary" margin={2}/> {timeFormatter(appointment.end)}
                </KFlexRow>
            }
            {appointment.employee &&
            <KFlexRow align="center">
                <KIcon icon="user" color="primary" margin={2}/> {appointment.employee.name}
            </KFlexRow>
            }
            {appointment.service &&
            <KFlexRow align="center">
                <KIcon icon="magic" color="primary" margin={2}/> {appointment.service.name}
            </KFlexRow>
            }
            {appointment.customer &&
            <KFlexRow align="center">
                <KIcon icon="address-card" color="primary" margin={2}/> {appointment.customer.name}
            </KFlexRow>
            }
            {appointment.internalNotes &&
            <KFlexRow align="center" className="mb-2">
                <KIcon icon="sticky-note" color="primary" margin={2}/> {appointment.internalNotes}
            </KFlexRow>
            }
            {/*{appointment.customerNotes &&*/}
            {/*<KFlexRow align="center" className="mb-2">*/}
            {/*    <KIcon icon="comment-alt" color="primary" margin={2}/> {appointment.customerNotes}*/}
            {/*</KFlexRow>*/}
            {/*}*/}
        </KCard>
    )
}

interface AppointmentHistoryContainerProps {
    id: string;
    isOpen: boolean;
    onClose: () => void;
}

const AppointmentHistoryContainer: React.FunctionComponent<AppointmentHistoryContainerProps> = (
    {
        id,
        isOpen,
        onClose
    }) => {
    const [appointments, setAppointments] = useState<AppointmentHistoryAdminResourceModel[]>([]);

    useEffect(() => {
        adminAppointmentClient
            .history(id)
            .then(res => {
                setAppointments(res.entities || []);
            });
    }, [id]);

    return (
        <KModal body={
            <KFlexColumn>
                {appointments.map(appointment => <AppointmentHistoryItem key={appointment.id}
                                                                         appointment={appointment}/>
                )}
            </KFlexColumn>
        }
                header="history"
                onCancel={onClose}
                isOpen={isOpen}>
        </KModal>
    )
}


export default AppointmentHistoryContainer;
