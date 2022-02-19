import React from 'react';
import {
    adminServiceClient,
    upsertServiceCommandParser,
    upsertServiceCommandValidation
} from 'src/app/api/adminServicesApi';
import {ServiceAdminResourceModel} from 'src/app/api/api';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import AdminButton from 'src/app/shared/admin/admin-button';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppSelector} from 'src/app/store';
import {serviceCategoryActions, serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';
import ServiceCategoryUpsertForm from './service-category-upsert-form';

const ServiceUpsertForm: React.FunctionComponent<AdminFormProps<ServiceAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const serviceCategories = useAppSelector(serviceCategorySelectors.selectAll)
    const [openModal, formModal] = useEditModal(serviceCategoryActions, ServiceCategoryUpsertForm);
    const {apiError, handleSubmit} = useHandleSubmit(adminServiceClient, entity, onSuccess);

    const serviceCategory = (id: string) => serviceCategories.find(sc => sc.id === id) || null

    return (
        <KFormikForm initialValues={upsertServiceCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
                     validationSchema={upsertServiceCommandValidation}
        >
            {(formik) =>
                <>
                    {formModal}
                    <KFlexRow align={'center'} justify={'center'}>
                        <KFormikInput className="flex-fill" name="serviceCategoryId" as={'select'} options={serviceCategories}/>
                        <AdminButton type={PermissionType.change}
                                     model={PermissionModel.serviceCategory}
                                     onClick={() => openModal(serviceCategory(formik.getFieldProps('category').value))}/>
                        <AdminButton type={PermissionType.add}
                                     model={PermissionModel.serviceCategory}
                                     onClick={() => openModal(null)}/>
                    </KFlexRow>
                    <KFormikInput name="name"/>
                    <KFormikInput name="duration" type="time"/>
                    <KFormikInput name="description"/>
                    <KFormikInput name="price" type="number"/>
                </>
            }

        </KFormikForm>
    )
}


export default ServiceUpsertForm;
