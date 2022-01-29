import React from 'react';
import {UpsertUserRequestValidation} from 'src/app/api/adminUsersApi';
import {UpsertUserCommand} from 'src/app/api/api';
import ChangePasswordForm from 'src/app/modules/admin/users/change-password-form';
import {useSelectAll} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {employeeActions, employeeSelectors} from 'src/app/store/admin/employees';
import {permissionGroupActions, permissionGroupSelectors} from 'src/app/store/admin/permissionGroups';


const UsersUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertUserCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        onCancel
    }) => {
    const employees = useSelectAll(employeeSelectors, employeeActions);
    const groups = useSelectAll(permissionGroupSelectors, permissionGroupActions);

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     onCancel={onCancel}
                     validationSchema={UpsertUserRequestValidation}
        >
            {id &&
            <ChangePasswordForm id={id}/>
            }
            <KFormikInput name="userName"/>
            <KFormikInput name="email"/>
            <KFormikInput name="roleGroupId" as={'select'} options={groups}/>
            <KFormikInput name="employeeId" as={'select'} options={employees}/>
        </KFormikForm>
    )
}


export default UsersUpsertForm;
