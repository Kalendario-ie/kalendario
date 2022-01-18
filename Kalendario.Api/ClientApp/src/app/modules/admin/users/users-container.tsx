import React from 'react';
import {PermissionModel} from 'src/app/api/auth';
import {upsertUserRequestParser} from 'src/app/api/users';
import UsersTable from 'src/app/modules/admin/users/users-table';
import UsersUpsertForm from 'src/app/modules/admin/users/users-upsert-form';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {useAppDispatch} from 'src/app/store';
import {customerActions} from 'src/app/store/admin/customers';
import {userActions, userSelectors} from 'src/app/store/admin/users';


const UsersContainer: React.FunctionComponent = () => {
    const dispatch = useAppDispatch()

    const filter = (search: string | undefined) => {
        dispatch(customerActions.fetchEntities({query: {search, start: 0, length: 200}}));
    }

    return (
            <AdminListEditContainer baseSelectors={userSelectors}
                                    baseActions={userActions}
                                    filter={filter}
                                    parser={upsertUserRequestParser}
                                    modelType={PermissionModel.user}
                                    EditContainer={UsersUpsertForm}
                                    ListContainer={UsersTable}/>
    )
}


export default UsersContainer;


