import React, {useMemo} from 'react';
import {EmployeeAdminResourceModel} from 'src/app/api/api';
import {Employee} from 'src/app/api/employees';
import EmployeeRowExpanded from 'src/app/modules/admin/employees/employee-row-expanded';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import KTable from 'src/app/shared/components/tables/k-table';
import {ExpanderColumn} from 'src/app/shared/components/tables/k-table-row-expander';
import KTextColumnFilter from 'src/app/shared/components/tables/k-text-column-filter';
import {useAppDispatch} from 'src/app/store';


const EmployeesTable: React.FunctionComponent<AdminTableContainerProps<EmployeeAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
        filter,
    }) => {
    const dispatch = useAppDispatch();


    const columns = useMemo(() => {
        const handleFileSubmit = (entity: Employee, file: File) => Promise.resolve(true) // todo fix here.
            // adminEmployeeClient.uploadProfilePicture(entity.id, file)
            //     .then(res => {
            //         dispatch(employeeReducerActions.upsertOne({...entity, photoUrl: res.url}));
            //         return true;
            //     })
            //     .catch(error => false);

        return [
            ExpanderColumn,
            // {
            //     Header: 'Photo',
            //     accessor: 'photoUrl',
            //     Cell: (value: any) => <EditableAvatarImg src={value.cell.value}
            //                                              onSubmit={(file) => handleFileSubmit(value.row.original, file)}
            //                                              size={3}/>
            // }, // todo add photo later.
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
