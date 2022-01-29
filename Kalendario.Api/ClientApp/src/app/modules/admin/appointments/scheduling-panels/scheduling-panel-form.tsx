import React from 'react';
import {UpsertSchedulingPanelCommand} from 'src/app/api/api';
import {useSelectAll} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {employeeActions, employeeSelectors} from 'src/app/store/admin/employees';

const SchedulingPanelForm: React.FunctionComponent<AdminEditContainerProps<UpsertSchedulingPanelCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        onCancel
    }) => {
    const employees = useSelectAll(employeeSelectors, employeeActions);

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     onCancel={onCancel}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="employeeIds" as="multi-select" options={employees}/>
        </KFormikForm>
    )
}


export default SchedulingPanelForm;
