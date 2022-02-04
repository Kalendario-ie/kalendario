import React, {useState} from 'react';
import {adminAppointmentClient} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import AppointmentHistoryContainer from 'src/app/modules/admin/appointments/appointment-history-container';
import DeleteButton from 'src/app/shared/admin/delete-button';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';
import {useAppDispatch} from 'src/app/store';
import {appointmentActions} from 'src/app/store/admin/appointments';
import {useInitializeServices} from 'src/app/store/admin/services';

interface AppointmentUpsertFormWrapperProps {
    entity: AppointmentAdminResourceModel | null;
    onDelete: () => void;
}

const AppointmentUpsertFormWrapper: React.FunctionComponent<AppointmentUpsertFormWrapperProps> = (
    {
        entity,
        onDelete,
        children
    }) => {
    const [showHistory, setShowHistory] = useState(false);
    useInitializeServices();
    const dispatch = useAppDispatch();

    const handleHistoryClick = () => {
        setShowHistory(true);
    };

    const handleHistoryCloseClick = () => {
        setShowHistory(false);
    };

    return (
        <>
            {entity && entity.id && entity.id !== '' &&
            <KFlexRow justify="end">
                <DeleteButton entity={entity}
                              onSuccess={() => {
                                  dispatch(appointmentActions.removeOne(entity.id));
                                  onDelete();
                              }}
                              modelType={PermissionModel.appointment}
                              client={adminAppointmentClient}/>
                <KIconButton icon="history"
                             color="primary"
                             onClick={handleHistoryClick}/>
                <AppointmentHistoryContainer id={entity.id} isOpen={showHistory} onClose={handleHistoryCloseClick}/>
            </KFlexRow>
            }
            {children}
        </>
    )
}


export default AppointmentUpsertFormWrapper;
