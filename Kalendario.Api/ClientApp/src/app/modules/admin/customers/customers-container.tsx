import React from 'react';
import {adminCustomerClient} from 'src/app/api/adminCustomerApi';
import {PermissionModel} from 'src/app/api/auth';
import CustomerUpsertForm from 'src/app/modules/admin/customers/customer-upsert-form';
import CustomersTable from 'src/app/modules/admin/customers/customers-table';
import {CUSTOMER_URLS} from 'src/app/modules/admin/customers/customers-urls';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {useAppDispatch, useAppSelector} from 'src/app/store';
import {customerActions, customerSelectors, useInitializeCustomers} from 'src/app/store/admin/customers';


const CustomersContainer: React.FunctionComponent = () => {
    const dispatch = useAppDispatch();
    useInitializeCustomers({length: undefined, search: undefined, start: undefined});
    const entities = useAppSelector(customerSelectors.selectAll)

    const filter = React.useMemo(() =>
        (search: string | undefined) => {
            adminCustomerClient.get({search, start: 0, length: 200})
                .then(result => {
                    dispatch(customerActions.setAll(result.entities || []));
                })
        }, []);

    return (
        <AdminListEditContainer actions={customerActions}
                                filter={filter}
                                client={adminCustomerClient}
                                modelType={PermissionModel.customer}
                                detailsUrl={CUSTOMER_URLS.DETAILS}
                                entities={entities}
                                EditContainer={CustomerUpsertForm}
                                ListContainer={CustomersTable}/>
    )
}


export default CustomersContainer;


