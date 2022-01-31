import moment from 'moment';
import React from 'react';
import {Spinner} from 'reactstrap';
import {upsertAppointmentCommandParser} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel, EmployeeAdminResourceModel, UpsertAppointmentCommand} from 'src/app/api/api';
import styles from 'src/app/modules/admin/appointments/employee-panel/employee-panel.module.scss';
import {useHoursConverter} from 'src/app/modules/admin/appointments/employee-panel/hooks';
import {compareByStartDate} from 'src/app/shared/util/comparers';
import {useAppSelector} from 'src/app/store';
import {appointmentSelectors} from 'src/app/store/admin/appointments';
import {serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';

interface EventProps {
    order: number;
    isOverlapping: boolean;
    appointment: AppointmentAdminResourceModel;
    onClick: () => void;
}

const BASE_WIDTH = 12.5;

const Event: React.FunctionComponent<EventProps> = (
    {
        order,
        isOverlapping,
        appointment,
        onClick
    }) => {
    const start = moment.utc(appointment.start);
    const end = moment.utc(appointment.end);
    const serviceCategory = useAppSelector(state => serviceCategorySelectors.selectById(state, appointment.service?.serviceCategoryId || ''));

    const duration = moment.duration(end.diff(start));

    const backgroundColor = serviceCategory ? serviceCategory.colour.code : '#FFFFFF';
    const title = appointment.customer ? appointment.customer.name : appointment.customerId ? 'Customer Deleted' : appointment.internalNotes;
    const subTitle = appointment.service ? appointment.service.name : appointment.serviceId ? 'Service Deleted' : '';

    const style: React.CSSProperties = {
        width: `${BASE_WIDTH - 0.25 - 3 * +isOverlapping}rem`,
        marginRight: '0.125rem',
        marginLeft: `${0.125 + 3 * +isOverlapping}rem`,
        zIndex: order + 2,
        top: useHoursConverter(start),
        height: useHoursConverter(duration),
        backgroundColor,
    }

    return (
        <div style={style}
             className={`${styles.panelEvent} k-shadow-0`}
             onClick={onClick}
        >
            <div>
                {title}
            </div>
            {subTitle}
        </div>
    )
}

interface EventsContainerProps {
    employee: EmployeeAdminResourceModel;
    onSelect: (entity: AppointmentAdminResourceModel) => void
}

const EventsContainer: React.FunctionComponent<EventsContainerProps> = (
    {
        employee,
        onSelect
    }) => {
    const appointments = useAppSelector(appointmentSelectors.selectAll);
    const isLoading = useAppSelector(appointmentSelectors.selectIsLoading);

    const employeeAppointments = React.useMemo(() =>
            appointments
                .filter(appointment => appointment.employee.id === employee.id)
                .sort(compareByStartDate)
        , [appointments, employee.id]
    )

    return (
        <div className="position-relative">
            {isLoading &&
            <Spinner className="position-absolute"/>
            }
            {employeeAppointments.map((appointment, index) =>
                <Event key={appointment.id}
                       isOverlapping={index > 0 ? isOverlapping(appointment, employeeAppointments[index - 1]) : false}
                       order={index}
                       appointment={appointment}
                       onClick={() => onSelect(appointment)}
                />
            )}
        </div>
    )
}

const isOverlapping = (currentAppointment: AppointmentAdminResourceModel, previousAppointment: AppointmentAdminResourceModel): boolean => {
    return moment.utc(currentAppointment.start).isBefore(moment.utc(previousAppointment.end));
}

export default EventsContainer;
