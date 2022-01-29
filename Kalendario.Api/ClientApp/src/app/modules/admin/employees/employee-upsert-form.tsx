import React, {useEffect} from 'react';
import {useSelector} from 'react-redux';
import {upsertEmployeeCommandParser, upsertEmployeeCommandValidation} from 'src/app/api/adminEmployeesApi';
import {EmployeeAdminResourceModel, UpsertEmployeeCommand} from 'src/app/api/api';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {useAppDispatch} from 'src/app/store';
import {scheduleActions, scheduleSelectors} from 'src/app/store/admin/schedules';
import {serviceCategoryActions} from 'src/app/store/admin/serviceCategories';
import {serviceActions, serviceSelectors} from 'src/app/store/admin/services';

const EmployeeUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertEmployeeCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        onCancel
    }) => {
    const schedules = useSelector(scheduleSelectors.selectAll)
    const services = useSelector(serviceSelectors.selectServicesWithCategories)

    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(scheduleActions.initializeStore());
        dispatch(serviceActions.initializeStore());
        dispatch(serviceCategoryActions.initializeStore());
    }, [dispatch]);

    return (
        <KFormikForm initialValues={command}
                     apiError={apiError}
                     onSubmit={(values => onSubmit(values, id))}
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
