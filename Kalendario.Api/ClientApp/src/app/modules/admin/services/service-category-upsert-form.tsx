import React from 'react';
import {ServiceCategoryAdminResourceModel} from 'src/app/api/api';
import {
    createUpsertServiceCategoryRequest,
    ServiceCategory,
    UpsertServiceCategoryRequestValidation
} from 'src/app/api/services';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const ServiceCategoryUpsertForm: React.FunctionComponent<AdminEditContainerProps<ServiceCategoryAdminResourceModel>> = (
    {
        entity,
        apiError,
        isSubmitting,
        onSubmit,
        onCancel
    }) => {
    return (
        <KFormikForm initialValues={createUpsertServiceCategoryRequest(entity)}
                     apiError={apiError}
                     onSubmit={onSubmit}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
                     validationSchema={UpsertServiceCategoryRequestValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="color" as="color"/>
        </KFormikForm>
    )
}


export default ServiceCategoryUpsertForm;
