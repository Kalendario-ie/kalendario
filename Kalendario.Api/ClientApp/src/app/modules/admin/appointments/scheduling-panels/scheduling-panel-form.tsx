import React from 'react';
import {adminSchedulingPanelsClient, upsertSchedulingPanelCommandParser} from 'src/app/api/adminSchedulingPanels';
import {SchedulingPanelAdminResourceModel} from 'src/app/api/api';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {employeeActions, employeeSelectors, useInitializeEmployees} from 'src/app/store/admin/employees';

const SchedulingPanelForm: React.FunctionComponent<AdminFormProps<SchedulingPanelAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const [, employees] = useInitializeEmployees();
    const {apiError, handleSubmit} = useHandleSubmit(adminSchedulingPanelsClient, entity, onSuccess);

    return (
        <KFormikForm initialValues={upsertSchedulingPanelCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="employeeIds" as="multi-select" options={employees}/>
        </KFormikForm>
    )
}


export default SchedulingPanelForm;
