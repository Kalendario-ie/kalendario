import React from 'react';
import {adminEmployeeClient, upsertEmployeeCommandParser} from 'src/app/api/adminEmployeesApi';
import {PermissionModel} from 'src/app/api/auth';
import EmployeeUpsertForm from 'src/app/modules/admin/employees/employee-upsert-form';
import EmployeesTable from 'src/app/modules/admin/employees/employees-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {employeeActions, employeeReducerActions, employeeSelectors} from 'src/app/store/admin/employees';


const EmployeesContainer: React.FunctionComponent = () => {
    return (
        <AdminListEditContainer baseSelectors={employeeSelectors}
                                baseActions={employeeActions}
                                actions={employeeReducerActions}
                                onCreate={adminEmployeeClient.post}
                                onUpdate={adminEmployeeClient.put}
                                parser={upsertEmployeeCommandParser}
                                modelType={PermissionModel.employee}
                                EditContainer={EmployeeUpsertForm}
                                ListContainer={EmployeesTable}/>
    )
}


export default EmployeesContainer;
