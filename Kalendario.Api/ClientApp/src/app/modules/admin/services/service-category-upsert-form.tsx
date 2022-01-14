import React from 'react';
import {UpsertServiceCategoryRequestValidation} from 'src/app/api/adminServiceCategoryApi';
import {UpsertServiceCategoryCommand} from 'src/app/api/api';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';

const ServiceCategoryUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertServiceCategoryCommand>> = (
    {
        id,
        command,
        apiError,
        isSubmitting,
        onSubmit,
        onCancel
    }) => {
    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
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
