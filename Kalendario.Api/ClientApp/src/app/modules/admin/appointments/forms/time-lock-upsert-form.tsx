import React from 'react';
import {
    adminTimeLockClient,
    upsertTimeLockCommandParser,
    upsertTimeLockCommandValidation
} from 'src/app/api/adminAppointments';
import {AppointmentAdminResourceModel} from 'src/app/api/api';
import AppointmentUpsertFormWrapper from 'src/app/modules/admin/appointments/forms/appointment-upsert-form-wrapper';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import KFormikStartEndTimeInput from 'src/app/shared/components/forms/k-formik-start-end-time-input';
import {useAppSelector} from 'src/app/store';
import {employeeSelectors} from 'src/app/store/admin/employees';


const TimeLockUpsertForm: React.FunctionComponent<AdminFormProps<AppointmentAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const employees = useAppSelector(employeeSelectors.selectAll);
    const {apiError, handleSubmit} = useHandleSubmit(adminTimeLockClient, entity, onSuccess);

    return (
        <AppointmentUpsertFormWrapper id={entity?.id}>
            <KFormikForm initialValues={upsertTimeLockCommandParser(entity)}
                         onSubmit={handleSubmit}
                         apiError={apiError}
                         onCancel={onCancel}
                         validationSchema={upsertTimeLockCommandValidation}
            >
                <KFormikStartEndTimeInput/>
                <KFormikInput name="employeeId" as={'select'} options={employees}/>
                <KFormikInput name="internalNotes" as={'textarea'}/>
                <KFormikInput placeholder="Allow Overlapping" name="ignoreTimeClashes" as={'checkbox'}/>
            </KFormikForm>
        </AppointmentUpsertFormWrapper>
    )
}

export default TimeLockUpsertForm;

