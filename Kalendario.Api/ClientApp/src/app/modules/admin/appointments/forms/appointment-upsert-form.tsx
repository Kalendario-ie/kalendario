import {useFormikContext} from 'formik';
import React, {useEffect, useState} from 'react';
import {upsertAppointmentCommandValidation} from 'src/app/api/adminAppointments';
import {UpsertAppointmentCommand} from 'src/app/api/api';
import AppointmentUpsertFormWrapper from 'src/app/modules/admin/appointments/forms/appointment-upsert-form-wrapper';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFormikCustomerInput, KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import KFormikStartEndTimeInput from 'src/app/shared/components/forms/k-formik-start-end-time-input';
import {compareByName} from 'src/app/shared/util/comparers';
import {addHours, stringToMoment} from 'src/app/shared/util/moment-helpers';
import {useAppSelector} from 'src/app/store';
import {appointmentSelectors} from 'src/app/store/admin/appointments';
import {employeeSelectors} from 'src/app/store/admin/employees';
import {serviceSelectors} from 'src/app/store/admin/services';


const UpdateEndTimeOnServiceChangeEffect: React.FunctionComponent = () => {
    const formik = useFormikContext();
    const serviceId = formik.getFieldProps<number>('serviceId').value;
    const start = formik.getFieldProps('start').value;
    const {setValue} = formik.getFieldHelpers('end');

    const [initialId, setInitialId] = useState(serviceId);
    const [initialStart, setInitialStart] = useState(start);

    const service = useAppSelector((state) => serviceSelectors.selectById(state, serviceId));

    useEffect(() => {
        if (service && (serviceId !== initialId || start !== initialStart)) {
            setInitialId(serviceId);
            setInitialStart(start);
            setValue(addHours(stringToMoment(start), service.duration))
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [initialId, start, service, serviceId, initialStart]);

    return null;
}


function ServicesInput() {
    const formik = useFormikContext();
    const employeeId = formik.getFieldProps<string>('employeeId').value;
    const [employeeServices, setEmployeeServices] = useState<string[]>([]);
    const employeeEntities = useAppSelector(employeeSelectors.selectEntities);
    const services = useAppSelector((state) => serviceSelectors
        .selectByIds(state, employeeServices))
        .sort(compareByName);

    useEffect(() => {
        setEmployeeServices(employeeEntities[employeeId]?.services || [])
    }, [employeeEntities, employeeId]);


    return (
        <KFormikInput name="serviceId" as={'select'} options={services}/>
    )
}


const AppointmentUpsertForm: React.FunctionComponent<AdminEditContainerProps<UpsertAppointmentCommand>> = (
    {
        id,
        command,
        apiError,
        onSubmit,
        onCancel
    }) => {
    const employees = useAppSelector(employeeSelectors.selectAll);
    const selectedAppointment = useAppSelector(store => appointmentSelectors.selectById(store, id || ''))

    return (
        <AppointmentUpsertFormWrapper id={id}>
            <KFormikForm initialValues={command}
                         onSubmit={(values => onSubmit(values, id))}
                         apiError={apiError}
                         onCancel={onCancel}
                         validationSchema={upsertAppointmentCommandValidation}
            >
                <UpdateEndTimeOnServiceChangeEffect/>
                <KFormikStartEndTimeInput/>
                <KFormikInput name="employeeId" as={'select'} options={employees}/>
                <ServicesInput/>
                <KFormikCustomerInput initialCustomer={selectedAppointment?.customer}/>
                <KFormikInput name="internalNotes" as={'textarea'}/>
                <KFormikInput placeholder="Allow Overlapping" name="ignoreTimeClashes" as={'checkbox'}/>
            </KFormikForm>
        </AppointmentUpsertFormWrapper>
    )
}


export default AppointmentUpsertForm;
