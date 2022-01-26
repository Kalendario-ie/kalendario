import React, {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {UpsertPermissionRequestValidation} from 'src/app/api/adminRoleGroupsApi';
import {UpsertApplicationRoleGroupCommand} from 'src/app/api/api';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppDispatch} from 'src/app/store';
import {permissionsActions, permissionSelectors} from 'src/app/store/admin/permissions';


const PermissionGroupUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertApplicationRoleGroupCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    const dispatch = useAppDispatch();
    const permissions = useSelector(permissionSelectors.selectAll)

    useEffect(() => {
        dispatch(permissionsActions.initializeStore());
    }, [dispatch]);

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
                     validationSchema={UpsertPermissionRequestValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="roles" as={'multi-select'} options={permissions}/>
        </KFormikForm>
    )
}


export default PermissionGroupUpsertForm;
