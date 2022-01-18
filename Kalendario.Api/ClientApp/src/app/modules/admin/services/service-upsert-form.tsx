import React from 'react';
import {upsertServiceCategoryCommandParser} from 'src/app/api/adminServiceCategoryApi';
import {upsertServiceCommandValidation} from 'src/app/api/adminServicesApi';
import {UpsertServiceCommand} from 'src/app/api/api';
import {PermissionModel, PermissionType} from 'src/app/api/auth';
import AdminButton from 'src/app/shared/admin/admin-button';
import {useEditModal} from 'src/app/shared/admin/hooks';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFlexRow} from 'src/app/shared/components/flex';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppSelector} from 'src/app/store';
import {serviceCategoryActions, serviceCategorySelectors} from 'src/app/store/admin/serviceCategories';
import ServiceCategoryUpsertForm from './service-category-upsert-form';

const ServiceUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertServiceCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        isSubmitting,
        onCancel
    }) => {
    const serviceCategories = useAppSelector(serviceCategorySelectors.selectAll)
    const [openModal, modal] = useEditModal(serviceCategorySelectors, serviceCategoryActions, ServiceCategoryUpsertForm);

    const serviceCategory = (id: string) => upsertServiceCategoryCommandParser(serviceCategories.find(sc => sc.id === id) || null)

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
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
                                     onClick={() => openModal(serviceCategory(formik.getFieldProps('category').value))}/>
                        <AdminButton type={PermissionType.add}
                                     model={PermissionModel.servicecategory}
                                     onClick={() => openModal(upsertServiceCategoryCommandParser(null))}/>
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
