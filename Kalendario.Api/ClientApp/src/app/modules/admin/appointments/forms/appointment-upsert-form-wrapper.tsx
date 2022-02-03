import React, {useState} from 'react';
import AppointmentHistoryContainer from 'src/app/modules/admin/appointments/appointment-history-container';
import {useInitializeEffect} from 'src/app/shared/admin/hooks';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KIconButton} from 'src/app/shared/components/primitives/buttons';
import {serviceActions} from 'src/app/store/admin/services';

interface AppointmentUpsertFormWrapperProps {
 id: string | undefined;
}

const AppointmentUpsertFormWrapper: React.FunctionComponent<AppointmentUpsertFormWrapperProps> = (
    {
        id,
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
            {id && id !== '' &&
            <KFlexRow justify="end">
                {/*<DeleteButton entity={entity}*/}
                {/*              modelType={PermissionModel.appointment}*/}
                {/*              baseActions={appointmentActions}/>*/}
                <KIconButton icon="history"
                             color="primary"
                             onClick={handleHistoryClick}/>
                <AppointmentHistoryContainer id={id} isOpen={showHistory} onClose={handleHistoryCloseClick}/>
            </KFlexRow>
            }
            {children}
        </>
    )
}


export default AppointmentUpsertFormWrapper;
