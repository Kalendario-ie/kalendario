import React from 'react';
import {adminUsersClient, upsertUserRequestParser, UpsertUserRequestValidation} from 'src/app/api/adminUsersApi';
import {ApplicationUserAdminResourceModel} from 'src/app/api/api';
import ChangePasswordForm from 'src/app/modules/admin/users/change-password-form';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useInitializeEmployees} from 'src/app/store/admin/employees';
import {useInitializePermissionGroups} from 'src/app/store/admin/permissionGroups';


const UsersUpsertForm: React.FunctionComponent<AdminFormProps<ApplicationUserAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    useInitializeEmployees();
    const [isEmployeesInitialized, employees] = useInitializeEmployees();
    const [_, groups] = useInitializePermissionGroups();
    const {apiError, handleSubmit} = useHandleSubmit(adminUsersClient, entity, onSuccess);

    return (
        <KFormikForm initialValues={upsertUserRequestParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
                     validationSchema={UpsertUserRequestValidation}
        >
            {entity?.id &&
            <ChangePasswordForm id={entity.id}/>
            }
            <KFormikInput name="userName"/>
            <KFormikInput name="email"/>
            <KFormikInput name="roleGroupId" as={'select'} options={groups}/>
            <KFormikInput name="employeeId" as={'select'} options={employees}/>
        </KFormikForm>
    )
}


export default UsersUpsertForm;
