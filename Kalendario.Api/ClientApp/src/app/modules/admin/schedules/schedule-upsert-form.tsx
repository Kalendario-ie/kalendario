import React from 'react';
import {FormGroup} from 'reactstrap';
import {ScheduleAdminResourceModel, UpsertScheduleCommand} from 'src/app/api/api';
import {Schedule, upsertScheduleRequestParser} from 'src/app/api/schedule';
import ScheduleFormikInput from 'src/app/modules/admin/schedules/schedule-shift-input/schedule-formik-input';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';


const ScheduleUpsertForm: React.FunctionComponent<AdminEditContainerProps<ScheduleAdminResourceModel, UpsertScheduleCommand>> = (
    {
        entity,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    return (
        <KFormikForm initialValues={upsertScheduleRequestParser(entity)}
                     apiError={apiError}
                     isSubmitting={isSubmitting}
                     onSubmit={(values => onSubmit(values, entity?.id))}
                     onCancel={onCancel}>
            <KFormikInput name="name"/>
            <FormGroup>
                <KFlexRow align={'center'} justify={'center'}>
                    <ScheduleFormikInput name="mon"/>
                    <ScheduleFormikInput name="tue"/>
                    <ScheduleFormikInput name="wed"/>
                    <ScheduleFormikInput name="thu"/>
                    <ScheduleFormikInput name="fri"/>
                    <ScheduleFormikInput name="sat"/>
                    <ScheduleFormikInput name="sun"/>
                </KFlexRow>
            </FormGroup>
        </KFormikForm>
    )
}


export default ScheduleUpsertForm;
