import React from 'react';
import {upsertCustomerCommandValidation} from 'src/app/api/adminCustomerApi';
import {UpsertCustomerCommand} from 'src/app/api/api';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const CustomerUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertCustomerCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
                     validationSchema={upsertCustomerCommandValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="email"/>
            <KFormikInput name="phoneNumber"/>
            <KFormikInput as="textarea" name="warning"/>
        </KFormikForm>
    )
}


export default CustomerUpsertForm;
