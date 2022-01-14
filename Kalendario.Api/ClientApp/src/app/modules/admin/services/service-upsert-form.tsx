import React from 'react';
import {ServiceAdminResourceModel, UpsertServiceCommand} from 'src/app/api/api';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import {createUpsertServiceCommand, upsertServiceCommandValidation} from 'src/app/api/adminServicesApi';
import AdminButton from 'src/app/shared/admin/admin-button';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppSelector} from 'src/app/store';
import {serviceCategoryActions, serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';
import ServiceCategoryUpsertForm from './service-category-upsert-form';

const ServiceUpsertForm: React.FunctionComponent<AdminEditContainerProps<ServiceAdminResourceModel, UpsertServiceCommand>> = (
    {
        entity,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    const serviceCategories = useAppSelector(serviceCategorySelectors.selectAll)
    const [openModal, modal] = useEditModal(serviceCategorySelectors, serviceCategoryActions, ServiceCategoryUpsertForm);

    const serviceCategory = (id: string) => serviceCategories.find(sc => sc.id === id) || null

    return (
        <KFormikForm initialValues={createUpsertServiceCommand(entity)}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, entity?.id))}
                     onCancel={onCancel}
                     isSubmitting={isSubmitting}
                     validationSchema={upsertServiceCommandValidation}
        >
            {(formik) =>
                <>
                    {modal}
                    <KFlexRow align={'center'} justify={'center'}>
                        <KFormikInput className="flex-fill" name="serviceCategoryId" as={'select'} options={serviceCategories}/>
                        <AdminButton type={PermissionType.change}
                                     model={PermissionModel.servicecategory}
                                     onClick={openModal(serviceCategory(formik.getFieldProps('category').value))}/>
                        <AdminButton type={PermissionType.add}
                                     model={PermissionModel.servicecategory}
                                     onClick={openModal(null)}/>
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
