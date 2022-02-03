import React from 'react';
import {
    adminServiceCategoryClient,
    upsertServiceCategoryCommandParser,
    UpsertServiceCategoryRequestValidation
} from 'src/app/api/adminServiceCategoryApi';
import {ServiceCategoryAdminResourceModel} from 'src/app/api/api';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const ServiceCategoryUpsertForm: React.FunctionComponent<AdminFormProps<ServiceCategoryAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const {apiError, handleSubmit} = useHandleSubmit(adminServiceCategoryClient, entity, onSuccess);

    return (
        <KFormikForm initialValues={upsertServiceCategoryCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
                     validationSchema={UpsertServiceCategoryRequestValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="colour" as="color"/>
        </KFormikForm>
    )
}


export default ServiceCategoryUpsertForm;
