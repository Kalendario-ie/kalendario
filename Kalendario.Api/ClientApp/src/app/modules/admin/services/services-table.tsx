import React, {useEffect, useMemo} from 'react';
import {ServiceAdminResourceModel} from 'src/app/api/api';
import {AdminTableContainerProps} from 'src/app/shared/admin/interfaces';
import {KSelectColumnFilter} from 'src/app/shared/components/tables/k-select-column-filter';
import KTable from 'src/app/shared/components/tables/k-table';
import KTextColumnFilter from 'src/app/shared/components/tables/k-text-column-filter';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {serviceCategoryActions, serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';


const ServicesTable: React.FunctionComponent<AdminTableContainerProps<ServiceAdminResourceModel>> = (
    {
        entities,
        buttonsColumn,
        filter,
    }) => {
    const serviceCategories = useAppSelector(serviceCategorySelectors.selectAll)
    const serviceCategoryDict = useAppSelector(serviceCategorySelectors.selectEntities)
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(serviceCategoryActions.initializeStore());
    }, [dispatch]);

    const columns = useMemo(
        () => [
            {
                Header: 'category',
                accessor: 'serviceCategoryId',
                Filter: (props: any) => <KSelectColumnFilter {...props} options={serviceCategories}/>,
                Cell: (value: any) => <>{serviceCategoryDict[value.cell.value]?.name}</>
            },
            {
                Header: 'Name',
                accessor: 'name',
                Filter: KTextColumnFilter
            },
            {
                Header: 'Duration',
                accessor: 'duration',
            },
            {
                Header: 'Description',
                accessor: 'description',
            },
            {
                Header: 'Price',
                accessor: 'price',
            },
            buttonsColumn
        ],
        [buttonsColumn, serviceCategories, serviceCategoryDict]
    )

    return (
        <KTable columns={columns} data={entities}/>
    )
}


export default ServicesTable;
