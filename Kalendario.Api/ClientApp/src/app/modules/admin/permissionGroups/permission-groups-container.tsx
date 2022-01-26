import React from 'react';
import {upsertPermissionGroupRequestParser} from 'src/app/api/adminRoleGroupsApi';
import {PermissionModel} from 'src/app/api/auth';
import PermissionGroupUpsertForm from 'src/app/modules/admin/permissionGroups/permission-group-upsert-form';
import PermissionGroupsTable from 'src/app/modules/admin/permissionGroups/permission-groups-table';
import AdminListEditContainer from 'src/app/shared/admin/admin-list-edit-container';
import {permissionGroupActions, permissionGroupSelectors} from 'src/app/store/admin/permissionGroups';


const PermissionGroupsContainer: React.FunctionComponent = () => {
    return (
            <AdminListEditContainer baseSelectors={permissionGroupSelectors}
                                    baseActions={permissionGroupActions}
                                    parser={upsertPermissionGroupRequestParser}
                                    modelType={PermissionModel.groupprofile}
                                    EditContainer={PermissionGroupUpsertForm}
                                    ListContainer={PermissionGroupsTable}/>
    )
}


export default PermissionGroupsContainer;


