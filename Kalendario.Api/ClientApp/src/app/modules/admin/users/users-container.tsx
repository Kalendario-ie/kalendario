import React from 'react';
import {adminUsersClient} from 'src/app/api/adminUsersApi';
import {PermissionModel} from 'src/app/api/auth';
import UsersTable from 'src/app/modules/admin/users/users-table';
import UsersUpsertForm from 'src/app/modules/admin/users/users-upsert-form';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {useInitializeUsers, userActions} from 'src/app/store/admin/users';


const UsersContainer: React.FunctionComponent = () => {
    const [, entities] = useInitializeUsers({length: undefined, search: undefined, start: undefined});

   return (
            <AdminListEditContainer entities={entities}
                                    actions={userActions}
                                    client={adminUsersClient}
                                    modelType={PermissionModel.user}
                                    EditContainer={UsersUpsertForm}
                                    ListContainer={UsersTable}/>
    )
}


export default UsersContainer;


