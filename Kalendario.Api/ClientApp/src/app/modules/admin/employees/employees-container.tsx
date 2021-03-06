import React from 'react';
import {adminEmployeeClient} from 'src/app/api/adminEmployeesApi';
import {PermissionModel} from 'src/app/api/auth';
import EmployeeUpsertForm from 'src/app/modules/admin/employees/employee-upsert-form';
import EmployeesTable from 'src/app/modules/admin/employees/employees-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {employeeActions, useInitializeEmployees} from 'src/app/store/admin/employees';


const EmployeesContainer: React.FunctionComponent = () => {
    const [, employees] = useInitializeEmployees();

    return (
        <AdminListEditContainer entities={employees}
                                actions={employeeActions}
                                client={adminEmployeeClient}
                                modelType={PermissionModel.employee}
                                EditContainer={EmployeeUpsertForm}
                                ListContainer={EmployeesTable}/>
    )
}


export default EmployeesContainer;
