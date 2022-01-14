import React, {useState} from 'react';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import AppointmentHistoryContainer from 'src/app/modules/admin/appointments/appointment-history-container';
import DeleteButton from 'src/app/shared/admin/delete-button';
import {useInitializeEffect} from 'src/app/shared/admin/hooks';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';
import {appointmentActions} from 'src/app/store/admin/appointments';
import {serviceActions} from 'src/app/store/admin/services';

interface AppointmentUpsertFormWrapperProps {
 entity: AppointmentAdminResourceModel;
}

const AppointmentUpsertFormWrapper: React.FunctionComponent<AppointmentUpsertFormWrapperProps> = (
    {
        entity,
        children
    }) => {
    const [showHistory, setShowHistory] = useState(false);
    useInitializeEffect(serviceActions);

    const handleHistoryClick = () => {
        setShowHistory(true);
    };

    const handleHistoryCloseClick = () => {
        setShowHistory(false);
    };

    return (
        <>
            {entity && entity.id !== '' &&
            <KFlexRow justify="end">
                <DeleteButton entity={entity}
                              modelType={PermissionModel.appointment}
                              baseActions={appointmentActions}/>
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
