import React from 'react';
import {UpsertUserRequest, UpsertUserRequestValidation} from 'src/app/api/adminUsersApi';
import ChangePasswordForm from 'src/app/modules/admin/users/change-password-form';
import {useSelectAll} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {employeeActions, employeeSelectors} from 'src/app/store/admin/employees';
import {permissionGroupActions, permissionGroupSelectors} from 'src/app/store/admin/permissionGroups';


const UsersUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertUserRequest>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    const employees = useSelectAll(employeeSelectors, employeeActions);
    const groups = useSelectAll(permissionGroupSelectors, permissionGroupActions);

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
                     validationSchema={UpsertUserRequestValidation}
        >
            {id &&
            <ChangePasswordForm id={id}/>
            }
            <KFormikInput name="userName"/>
            <KFormikInput name="email"/>
            <KFormikInput name="roleGroup" as={'select'} options={groups}/>
            <KFormikInput name="employee" as={'select'} options={employees}/>
        </KFormikForm>
    )
}


export default UsersUpsertForm;
