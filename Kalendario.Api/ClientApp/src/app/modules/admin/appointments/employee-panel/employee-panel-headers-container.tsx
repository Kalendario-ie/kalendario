import React from 'react';
import {AppointmentAdminResourceModel, EmployeeAdminResourceModel} from 'src/app/api/api';
import CreateAppointmentButtons from 'src/app/modules/admin/appointments/employee-panel/create-appointment-buttons';
import {useSelectPanelEmployees} from 'src/app/modules/admin/appointments/employee-panel/hooks';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import AvatarImg from 'src/app/shared/components/primitives/avatar-img';
import KFiller from 'src/app/shared/components/primitives/k-filler';
import {useAppSelector} from 'src/app/store';
import {adminDashboardSelectors} from 'src/app/store/admin/dashboard';
import styles from './employee-panel.module.scss';

interface EmployeePanelHeaderProps {
    employee: EmployeeAdminResourceModel;
    onCreateClick: (entity: AppointmentAdminResourceModel) => void;
    onCreateLockClick: (entity: AppointmentAdminResourceModel) => void;
}

const EmployeePanelHeader: React.FunctionComponent<EmployeePanelHeaderProps> = (
    {
        employee,
        onCreateClick,
        onCreateLockClick
    }) => {
    const currentDate = useAppSelector(adminDashboardSelectors.selectCurrentDate);

    return (
        <KFlexColumn className={`${styles.panelItem} py-3`} align={'center'} justify={'center'}>
            {employee.name}
            <AvatarImg className="m-1" size={4} key={employee.id} src={employee.photoUrl || ''}/>
            <KFlexRow>
                <CreateAppointmentButtons employee={employee}
                                          currentDate={currentDate}
                                          onCreateClick={onCreateClick}
                                          onCreateLockClick={onCreateLockClick}
                                          hour={0}
                                          minute={0}/>
            </KFlexRow>
        </KFlexColumn>
    )
}

interface EmployeePanelHeadersContainerProps {
    onCreateClick: (entity: AppointmentAdminResourceModel) => void;
    onCreateLockClick: (entity: AppointmentAdminResourceModel) => void;
}

export const EmployeePanelHeadersContainer: React.FunctionComponent<EmployeePanelHeadersContainerProps> = (
    {
        onCreateClick,
        onCreateLockClick
    }) => {
    const employees = useSelectPanelEmployees();

    return (
        <KFlexRow>
            <KFiller className={`sticky-top-left bg-white-gray ${styles.borderRight}`}>
                <div className={styles.sideItem}/>
            </KFiller>
            {employees.map(employee => <EmployeePanelHeader key={employee.id}
                                                            employee={employee}
                                                            onCreateClick={onCreateClick}
                                                            onCreateLockClick={onCreateLockClick}/>
            )}
        </KFlexRow>
    )
}


