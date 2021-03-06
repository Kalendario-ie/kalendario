import React, {useMemo} from 'react';
import {adminEmployeeClient} from 'src/app/api/adminEmployeesApi';
import {EmployeeAdminResourceModel} from 'src/app/api/api';
import EmployeeRowExpanded from 'src/app/modules/admin/employees/employee-row-expanded';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import EditableAvatarImg from 'src/app/shared/components/primitives/containers/editable-avatar-img';
import KTable from 'src/app/shared/components/tables/k-table';
import {ExpanderColumn} from 'src/app/shared/components/tables/k-table-row-expander';
import KTextColumnFilter from 'src/app/shared/components/tables/k-text-column-filter';
import {useAppDispatch} from 'src/app/store';
import {employeeActions} from 'src/app/store/admin/employees';

const EmployeesTable: React.FunctionComponent<AdminTableContainerProps<EmployeeAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
        filter,
    }) => {
    const dispatch = useAppDispatch();


    const columns = useMemo(() => {
        const handleFileSubmit = (entity: EmployeeAdminResourceModel, file: File) => adminEmployeeClient
                .employeesUploadFile(entity.id, file)
                .then(res => {
                    dispatch(employeeActions.upsertOne(res));
                    return true;
                }).catch(error => false);

        return [
            ExpanderColumn,
            {
                Header: 'Photo',
                accessor: 'photoUrl',
                Cell: (value: any) => <EditableAvatarImg src={value.cell.value}
                                                         onSubmit={(file) => handleFileSubmit(value.row.original, file)}
                                                         size={3}/>
            },
            {
                Header: 'Name',
                accessor: 'name',
                Filter: KTextColumnFilter
            },
            {
                Header: 'Email',
                accessor: 'email',
                Filter: KTextColumnFilter
            },
            {
                Header: 'Phone',
                accessor: 'phoneNumber',
                Filter: KTextColumnFilter
            },
            buttonsColumn
        ]
    }, [buttonsColumn, dispatch])


    const renderRowSubComponent = React.useCallback(
        (row: any) => <EmployeeRowExpanded employee={row.original}/>, [])

    return (
        <KTable columns={columns}
                data={entities}
                renderRowSubComponent={renderRowSubComponent}
                hover={true}
        />
    )
}


export default EmployeesTable;
