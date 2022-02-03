import moment from 'moment';
import React, {useContext, useEffect, useState} from 'react';
import {AppointmentAdminResourceModel, CustomerAdminResourceModel} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import AppointmentUpsertForm from 'src/app/modules/admin/appointments/forms/appointment-upsert-form';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {useSelectAll} from 'src/app/shared/admin/hooks';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import {KDateInput} from 'src/app/shared/components/primitives/inputs';
import {KSelectColumnFilter} from 'src/app/shared/components/tables/k-select-column-filter';
import KTable from 'src/app/shared/components/tables/k-table';
import {useAppDispatch} from 'src/app/store';
import {adminAppointmentSlice, appointmentActions, appointmentSelectors} from 'src/app/store/admin/appointments';
import {employeeActions, employeeSelectors} from 'src/app/store/admin/employees';
import {serviceActions, serviceSelectors} from 'src/app/store/admin/services';


const CustomerAppointmentsTable: React.FunctionComponent<AdminTableContainerProps<AppointmentAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
        filter,
    }) => {
    const [start, setStart] = useState(moment.utc().subtract(1, 'week').startOf('day'));
    const [end, setEnd] = useState(moment.utc().add(1, 'week').startOf('day'));

    const services = useSelectAll(serviceSelectors, serviceActions);
    const employees = useSelectAll(employeeSelectors, employeeActions);

    const dispatch = useAppDispatch();

    const customer = useContext(CustomerContext);

    useEffect(() => {
        dispatch(appointmentActions
            .fetchEntitiesWithSetAll({
                query: {
                    fromDate: start.toISOString(),
                    toDate: end.toISOString(),
                    customerId: customer?.id,
                    employeeIds: []
                }
            }));
    }, [start, end, customer, dispatch]);

    const columns = React.useMemo(() => [
        {
            Header: 'Start',
            accessor: 'start',
            Filter: (cell: any) => <KDateInput value={start} onChange={(value) => setStart(value)}/>
        },
        {
            Header: 'End',
            accessor: 'end',
            Filter: (cell: any) => <KDateInput value={end} onChange={(value) => setEnd(value)}/>
        },
        {
            Header: 'Employee',
            accessor: 'employee.name',
            Filter: (props: any) => <KSelectColumnFilter {...props} options={employees}/>,
        },
        {
            Header: 'Service',
            accessor: 'service.name',
            Filter: (props: any) => <KSelectColumnFilter {...props} options={services}/>,
        },
        {
            Header: 'Notes',
            accessor: 'internalNotes',
        },
        buttonsColumn
        // eslint-disable-next-line react-hooks/exhaustive-deps
    ], [employees, services, start, end]);


    return (
        <KTable columns={columns}
                data={entities}/>
    )
}


const CustomerContext = React.createContext<CustomerAdminResourceModel | null>(null);

interface CustomerAppointmentsProps {
    customer: CustomerAdminResourceModel;
}

const CustomerAppointments: React.FunctionComponent<CustomerAppointmentsProps> = (
    {
        customer
    }) => {
    return (

        <CustomerContext.Provider value={customer}>
            <AdminListEditContainer baseSelectors={appointmentSelectors}
                                    baseActions={appointmentActions}
                                    actions={adminAppointmentSlice.actions}
                                    initializeStore={false}
                                    modelType={PermissionModel.appointment}
                                    EditContainer={AppointmentUpsertForm}
                                    ListContainer={CustomerAppointmentsTable}/>
        </CustomerContext.Provider>
    )
}


export default CustomerAppointments;
