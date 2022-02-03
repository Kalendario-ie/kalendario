import React, {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {
    adminEmployeeClient,
    upsertEmployeeCommandParser,
    upsertEmployeeCommandValidation
} from 'src/app/api/adminEmployeesApi';
import {EmployeeAdminResourceModel} from 'src/app/api/api';
import {AdminFormProps, useHandleSubmit} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppDispatch} from 'src/app/store';
import {scheduleActions, scheduleSelectors} from 'src/app/store/admin/schedules';
import {serviceCategoryActions} from 'src/app/store/admin/serviceCategories';
import {serviceActions, serviceSelectors} from 'src/app/store/admin/services';

const EmployeeUpsertForm: React.FunctionComponent<AdminFormProps<EmployeeAdminResourceModel>> = (
    {
        entity,
        onSuccess,
        onCancel
    }) => {
    const schedules = useSelector(scheduleSelectors.selectAll)
    const services = useSelector(serviceSelectors.selectServicesWithCategories)
    const {apiError, handleSubmit} = useHandleSubmit(adminEmployeeClient, entity, onSuccess);

    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(scheduleActions.initializeStore());
        dispatch(serviceActions.initializeStore());
        dispatch(serviceCategoryActions.initializeStore());
    }, [dispatch]);

    return (
        <KFormikForm initialValues={upsertEmployeeCommandParser(entity)}
                     apiError={apiError}
                     onSubmit={handleSubmit}
                     onCancel={onCancel}
                     validationSchema={upsertEmployeeCommandValidation}
        >
            <KFormikInput name="name"/>
            <KFormikInput name="email"/>
            <KFormikInput name="phoneNumber"/>
            <KFormikInput name="scheduleId" as={'select'} options={schedules}/>
            <KFormikInput name="services" as={'multi-select'} options={services}/>
        </KFormikForm>
    )
}


export default EmployeeUpsertForm;
