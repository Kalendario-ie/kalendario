import React from 'react';
import {getFramesForDate, isAvailable} from 'src/app/api/adminSchedulesApi';
import {AppointmentAdminResourceModel, EmployeeAdminResourceModel} from 'src/app/api/api';
import {useSelectPanelEmployees} from 'src/app/modules/admin/appointments/employee-panel/hooks';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import KShowOnHoverContainer from 'src/app/shared/components/primitives/containers/k-show-on-hover-container';
import {useAppSelector} from 'src/app/store';
import {adminDashboardSelectors} from 'src/app/store/admin/dashboard';
import {scheduleSelectors} from 'src/app/store/admin/schedules';
import CreateAppointmentButtons from './create-appointment-buttons';
import styles from './employee-panel.module.scss';
import EventsContainer from './event-container';

const PanelHours: React.FunctionComponent = () => {
    const hours = useAppSelector(adminDashboardSelectors.selectPanelHours);
    const slotSize = useAppSelector(adminDashboardSelectors.selectSlotSize);

    const style: React.CSSProperties = {
        minHeight: `${slotSize / 2}rem`,
        height: `${slotSize / 2}rem`,
        textAlign: 'right',
        position: 'relative',
        top: '-0.75rem'
    }
    return (
        <KFlexColumn className={`sticky-top-left bg-white-gray ${styles.borderRight}`}>
            {hours.map((hour, i) =>
                <React.Fragment key={i}>
                    <div style={style} className={styles.sideItem}>
                        {hour}
                    </div>
                    <div style={style}/>
                </React.Fragment>
            )}
        </KFlexColumn>
    )
}

interface EmployeePanelProps {
    employee: EmployeeAdminResourceModel;
    onCreateClick: (entity: AppointmentAdminResourceModel) => void;
    onCreateLockClick: (entity: AppointmentAdminResourceModel) => void;
}

const EmployeePanelBody: React.FunctionComponent<EmployeePanelProps> = (
    {
        employee,
        onCreateClick,
        onCreateLockClick
    }) => {
    const currentDate = useAppSelector(adminDashboardSelectors.selectCurrentDate);
    const schedule = useAppSelector(state => scheduleSelectors.selectById(state, employee.scheduleId || ''));
    const hours = useAppSelector(adminDashboardSelectors.selectPanelHours);
    const slotSize = useAppSelector(adminDashboardSelectors.selectSlotSize);
    const style = {
        height: `${slotSize / 2}rem`,
    }

    function backgroundColor(hour: number, minute: number) {
        return schedule && getFramesForDate(schedule, currentDate)
            .some(frame => isAvailable(frame, hour, minute)) ? '' : styles.unavailableSlot;
    }

    return (
        <KFlexColumn>
            {hours.map((hour, i) =>
                <React.Fragment key={i}>
                    <KShowOnHoverContainer className={`${styles.middleItem} ${backgroundColor(hour, 0)}`}
                                           style={style}
                    >
                        <CreateAppointmentButtons employee={employee}
                                                  onCreateClick={onCreateClick}
                                                  onCreateLockClick={onCreateLockClick}
                                                  currentDate={currentDate}
                                                  hour={hour}
                                                  minute={0}/>
                    </KShowOnHoverContainer>
                    <KShowOnHoverContainer className={`${styles.middleItem} ${backgroundColor(hour, 30)}`}
                                           style={style}
                    >
                        <CreateAppointmentButtons employee={employee}
                                                  onCreateClick={onCreateClick}
                                                  onCreateLockClick={onCreateLockClick}
                                                  currentDate={currentDate}
                                                  hour={hour}
                                                  minute={30}/>
                    </KShowOnHoverContainer>
                </React.Fragment>
            )}
        </KFlexColumn>
    )
}

export interface EmployeePanelsBodyContainerProps {
    onSelect: (entity: AppointmentAdminResourceModel) => void;
    onCreateClick: (command: AppointmentAdminResourceModel) => void;
    onLockClick: (command: AppointmentAdminResourceModel) => void;
}

export const EmployeePanelsBodyContainer: React.FunctionComponent<EmployeePanelsBodyContainerProps> = ({onSelect, onCreateClick, onLockClick}) => {
    const employees = useSelectPanelEmployees();
    return (
        <KFlexRow>
            <PanelHours/>
            {employees.map(employee =>
                <React.Fragment key={employee.id}>
                    <EventsContainer onSelect={onSelect} employee={employee}/>
                    <EmployeePanelBody onCreateClick={onCreateClick} onCreateLockClick={onLockClick} employee={employee}/>
                </React.Fragment>
            )}
        </KFlexRow>
    )
}

