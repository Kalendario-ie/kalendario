import React from 'react';
import {ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand} from 'src/app/api/api';
import {
    createUpsertServiceCategoryRequest,
    UpsertServiceCategoryRequestValidation
} from 'src/app/api/adminServiceCategoryApi';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const ServiceCategoryUpsertForm: React.FunctionComponent<AdminEditContainerProps<ServiceCategoryAdminResourceModel, UpsertServiceCategoryCommand>> = (
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
                     onSubmit={(values => onSubmit(values, entity?.id))}
                     isSubmitting={isSubmitting}
                     onCancel={onCancel}
                     validationSchema={UpsertServiceCategoryRequestValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="colour" as="color"/>
        </KFormikForm>
    )
}


export default ServiceCategoryUpsertForm;
