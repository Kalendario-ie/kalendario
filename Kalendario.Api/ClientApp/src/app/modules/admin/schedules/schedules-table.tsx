import {ScheduleAdminResourceModel} from 'src/app/api/api';
import {Schedule} from 'src/app/api/schedule';
import React, {useMemo} from 'react';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import KTable from 'src/app/shared/components/tables/k-table';
import KTextColumnFilter from 'src/app/shared/components/tables/k-text-column-filter';
import ShiftCell from './shift-cell';

const SchedulesTable: React.FunctionComponent<AdminTableContainerProps<ScheduleAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
    }) => {
    const columns = useMemo(
        () => [
            {
                Header: 'Name',
                accessor: 'name',
                Filter: KTextColumnFilter
            },
            ...['monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday', 'sunday'].map(day => (
                {
                    Header: day.toUpperCase(),
                    accessor: day,
                    Cell: (value: any) => <ShiftCell shift={value.cell.value}/>
                }
            )),
            buttonsColumn
        ],
        [buttonsColumn]
    )

    return (
        <KTable columns={columns} data={entities}/>
    )
}


export default SchedulesTable;
