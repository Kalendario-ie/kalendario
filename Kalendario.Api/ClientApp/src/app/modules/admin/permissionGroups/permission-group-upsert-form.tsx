import React, {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {
    adminPermissionGroupClient,
    upsertPermissionGroupRequestParser,
    UpsertPermissionRequestValidation
} from 'src/app/api/adminRoleGroupsApi';
import {RoleGroupAdminResourceModel} from 'src/app/api/api';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppDispatch} from 'src/app/store';
import {permissionsActions, permissionSelectors} from 'src/app/store/admin/permissions';


const PermissionGroupUpsertForm: React.FunctionComponent<AdminFormProps<RoleGroupAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const dispatch = useAppDispatch();
    const permissions = useSelector(permissionSelectors.selectAll)
    const {apiError, handleSubmit} = useHandleSubmit(adminPermissionGroupClient, entity, onSuccess);

    useEffect(() => {
        dispatch(permissionsActions.initializeStore());
    }, [dispatch]);

    return (
        <KFormikForm initialValues={upsertPermissionGroupRequestParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
                     validationSchema={UpsertPermissionRequestValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="roles" as={'multi-select'} options={permissions}/>
        </KFormikForm>
    )
}


export default PermissionGroupUpsertForm;
