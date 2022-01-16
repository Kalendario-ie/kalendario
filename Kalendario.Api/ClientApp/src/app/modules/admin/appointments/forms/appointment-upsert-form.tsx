import {useFormikContext} from 'formik';
import moment, {Moment} from 'moment';
import React, {ChangeEvent, useEffect, useState} from 'react';
import {FormGroup, Input, Label} from 'reactstrap';
import {upsertAppointmentCommandValidation} from 'src/app/api/adminAppointments';
import {UpsertAppointmentCommand} from 'src/app/api/api';
import AppointmentUpsertFormWrapper from 'src/app/modules/admin/appointments/forms/appointment-upsert-form-wrapper';
import {AdminEditContainerProps} from 'src/app/shared/admin/interfaces';
import {KFlexColumn, KFlexRow} from 'src/app/shared/components/flex';
import {KFormikCustomerInput, KFormikForm, KFormikInput} from 'src/app/shared/components/forms';
import {KDateInput} from 'src/app/shared/components/primitives/inputs';
import {compareByName} from 'src/app/shared/util/comparers';
import {stringToMoment} from 'src/app/shared/util/moment-helpers';
import {useAppSelector} from 'src/app/store';
import {appointmentSelectors} from 'src/app/store/admin/appointments';
import {employeeSelectors} from 'src/app/store/admin/employees';
import {serviceSelectors} from 'src/app/store/admin/services';

function addHours(date: Moment, time: string): string {
    const momentTime = moment.utc(time, 'HH:mm')
    return date.clone()
        .add(momentTime.hour(), 'hour')
        .add(momentTime.minutes(), 'minutes')
        .toISOString();
}

function useDateHelper(name: string): [Moment, (value: Moment) => void, string, (event: ChangeEvent<HTMLInputElement>) => void] {
    const formik = useFormikContext();
    const {value} = formik.getFieldMeta<string>(name);
    const {setValue} = formik.getFieldHelpers(name);

    const momentValue = stringToMoment(value);
    const [time, setTime] = useState(momentValue.format('HH:mm'));

    useEffect(() => {
        const momentValue = stringToMoment(value);
        setTime(momentValue.format('HH:mm'))
    }, [value]);

    const handleDateChange = (value: Moment) => {
        setValue((addHours(value.startOf('day'), time)));

    }
    const handleTimeChange = (e: ChangeEvent<HTMLInputElement>) => {
        setTime(e.target.value);
        setValue((addHours(momentValue.startOf('day'), e.target.value)));

    }


    return [momentValue, handleDateChange, time, handleTimeChange]
}

function useUpdateEndTimeOnServiceChangeEffect() {
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
}


const FormikStartEndTimeInput: React.FunctionComponent = () => {
    const [start, handleDateChange, startTime, handleStartTimeChange] = useDateHelper('start');
    const [, handleEndDateChange, endTime, handleEndTimeChange] = useDateHelper('end');
    useUpdateEndTimeOnServiceChangeEffect();

    return (
        <>
            <FormGroup>
                <KFlexColumn>
                    <Label>Date</Label>
                    <KDateInput value={start}
                                onChange={(e) => {
                                    handleDateChange(e);
                                    handleEndDateChange(e);
                                }}/>
                </KFlexColumn>
            </FormGroup>
            <FormGroup>
                <KFlexRow align={'center'} justify={'center'}>
                    <KFlexColumn className="w-100">
                        Start
                        <Input value={startTime} onChange={handleStartTimeChange} type={'time'}/>
                    </KFlexColumn>
                    <KFlexColumn className="w-100">
                        Finish
                        <Input value={endTime} onChange={handleEndTimeChange} type={'time'}/>
                    </KFlexColumn>
                </KFlexRow>
            </FormGroup>
        </>
    )
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
        isSubmitting,
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
                         isSubmitting={isSubmitting}
                         onCancel={onCancel}
                         validationSchema={upsertAppointmentCommandValidation}
            >
                <FormikStartEndTimeInput/>
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
