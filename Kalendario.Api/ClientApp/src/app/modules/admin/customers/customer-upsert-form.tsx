import React from 'react';
import {
    adminCustomerClient,
    upsertCustomerCommandParser,
    upsertCustomerCommandValidation
} from 'src/app/api/adminCustomerApi';
import {CustomerAdminResourceModel} from 'src/app/api/api';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const CustomerUpsertForm: React.FunctionComponent<AdminFormProps<CustomerAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const {apiError, handleSubmit} = useHandleSubmit(adminCustomerClient, entity, onSuccess);

    return (
        <KFormikForm initialValues={upsertCustomerCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
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
