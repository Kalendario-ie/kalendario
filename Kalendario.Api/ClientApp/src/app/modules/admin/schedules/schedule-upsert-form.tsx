import React from 'react';
import {FormGroup} from 'reactstrap';
import {UpsertScheduleCommand} from 'src/app/api/api';
import ScheduleFormikInput from 'src/app/modules/admin/schedules/schedule-shift-input/schedule-formik-input';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';


const ScheduleUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertScheduleCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        onCancel
    }) => {
    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     onCancel={onCancel}>
            <KFormikInput name="name"/>
            <FormGroup>
                <ScheduleFormikInput name="monday"/>
                <ScheduleFormikInput name="tuesday"/>
                <ScheduleFormikInput name="wednesday"/>
                <ScheduleFormikInput name="thursday"/>
                <ScheduleFormikInput name="friday"/>
                <ScheduleFormikInput name="saturday"/>
                <ScheduleFormikInput name="sunday"/>
            </FormGroup>
        </KFormikForm>
    )
}


export default ScheduleUpsertForm;
