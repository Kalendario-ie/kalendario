import React from 'react';
import {upsertSchedulingPanelCommandParser} from 'src/app/api/adminSchedulingPanels';
import {SchedulingPanelAdminResourceModel, UpsertSchedulingPanelCommand} from 'src/app/api/api';
import {useSelectAll} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {employeeActions, employeeSelectors} from 'src/app/store/admin/employees';

const SchedulingPanelForm: React.FunctionComponent<AdminEditContainerProps<SchedulingPanelAdminResourceModel, UpsertSchedulingPanelCommand>> = (
    {
        entity,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    const employees = useSelectAll(employeeSelectors, employeeActions);

    return (
        <KFormikForm initialValues={upsertSchedulingPanelCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, entity?.id.toString()))}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="employeeIds" as="multi-select" options={employees}/>
        </KFormikForm>
    )
}


export default SchedulingPanelForm;
