import moment from 'moment';
import React, {useContext, useEffect, useState} from 'react';
import {adminAppointmentClient} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel, CustomerAdminResourceModel} from 'src/app/api/api';
import {PermissionModel} from 'src/app/api/auth';
import AppointmentUpsertForm from 'src/app/modules/admin/appointments/forms/appointment-upsert-form';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import {KDateInput} from 'src/app/shared/components/primitives/inputs';
import {KSelectColumnFilter} from 'src/app/shared/components/tables/k-select-column-filter';
import KTable from 'src/app/shared/components/tables/k-table';
import {useAppDispatch} from 'src/app/store';
import {appointmentActions} from 'src/app/store/admin/appointments';
import {useInitializeEmployees} from 'src/app/store/admin/employees';
import {useInitializeServices} from 'src/app/store/admin/services';


const CustomerAppointmentsTable: React.FunctionComponent<AdminTableContainerProps<AppointmentAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
        filter,
    }) => {
    const [start, setStart] = useState(moment.utc().subtract(1, 'week').startOf('day'));
    const [end, setEnd] = useState(moment.utc().add(1, 'week').startOf('day'));

    const [, services] = useInitializeServices();
    const [, employees] = useInitializeEmployees();

    const dispatch = useAppDispatch();

    const customer = useContext(CustomerContext);

    useEffect(() => {
        adminAppointmentClient.get({
            fromDate: start.toISOString(),
            toDate: end.toISOString(),
            customerId: customer?.id,
            employeeIds: []
        }).then(res => dispatch(appointmentActions.setAll(res.entities || [])))
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
            <AdminListEditContainer entities={[]}
                                    client={adminAppointmentClient}
                                    actions={appointmentActions}
                                    modelType={PermissionModel.appointment}
                                    EditContainer={AppointmentUpsertForm}
                                    ListContainer={CustomerAppointmentsTable}/>
        </CustomerContext.Provider>
    )
}


export default CustomerAppointments;
