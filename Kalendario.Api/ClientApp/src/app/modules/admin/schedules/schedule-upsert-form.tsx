import React from 'react';
import {FormGroup} from 'reactstrap';
import {adminScheduleClient, upsertScheduleCommandParser} from 'src/app/api/adminSchedulesApi';
import {ScheduleAdminResourceModel} from 'src/app/api/api';
import ScheduleFormikInput from 'src/app/modules/admin/schedules/schedule-shift-input/schedule-formik-input';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';


const ScheduleUpsertForm: React.FunctionComponent<AdminFormProps<ScheduleAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const {apiError, handleSubmit} = useHandleSubmit(adminScheduleClient, entity, onSuccess);

    return (
        <KFormikForm initialValues={upsertScheduleCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
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
