import {CancelToken} from 'axios';
import {Moment} from 'moment';
import * as moment from 'moment';
import {AppointmentAdminResourceModel, AppointmentsClient, UpsertAppointmentCommand} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new AppointmentsClient('', baseApiAxios);

export interface AppointmentsGetParams {
    fromDate: moment.Moment | undefined;
    toDate: moment.Moment | undefined;
    customerId: string | undefined;
    employeeIds: string[];
    cancelToken?: CancelToken | undefined;
}

export const adminAppointmentClient: BaseModelRequest<AppointmentAdminResourceModel, UpsertAppointmentCommand, AppointmentsGetParams> = {
    get(params) {
        return client.appointmentsGet(params?.fromDate, params?.toDate, params?.customerId, params?.employeeIds, params?.cancelToken);
    },
    post(body: UpsertAppointmentCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.appointmentsPost(body, cancelToken);
    },
    put(id: string, command: UpsertAppointmentCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.appointmentsPut(id, command, cancelToken);
    }
    // ...baseModelRequest<Appointment, AppointmentQueryParams>(adminUrl, appointmentParser),
    // history(id: number): Promise<ApiListResult<AppointmentHistory>> {
    //     return baseApiAxios.get<ApiListResult<AppointmentHistory>>(adminUrl + `${id}/history/`)
    //         .then(project => {
    //             project.data.results = project.data.results.map(appointmentHistoryParser);
    //             return project.data;
    //         });
    // },
    //
    // createLock(model: any): Promise<Appointment> {
    //     return baseApiAxios.post<Appointment>(adminUrl + 'lock/', model)
    //         .then(result => customerRequestAppointmentParser(result.data));
    // },
    //
    // updateLock(id: number, model: UpsertCustomerAppointmentRequest): Promise<Appointment> {
    //     return baseApiAxios.patch<Appointment>(adminUrl + `${id}/plock/`, model)
    //         .then(result => customerRequestAppointmentParser(result.data));
    // }
}


export function upsertAppointmentCommandParser(appointment: AppointmentAdminResourceModel | null): UpsertAppointmentCommand {
    return appointment == null ? {
        start: moment.utc(),
        end: moment.utc(),
        customerId: '',
        employeeId: '',
        serviceId: '',
        ignoreTimeClashes: false,
        internalNotes: '',
    } : {
        internalNotes: appointment.internalNotes,
        ignoreTimeClashes: false,
        start: appointment.start,
        end: appointment.end,
        employeeId: appointment.employee.id,
        customerId: appointment.customer.id,
        serviceId: appointment.service.id,
    }
}

export const upsertAppointmentCommandValidation = yup.object().shape({
    internalNotes: yup.string(),
    employeeId: yup.string().required(),
    customerId: yup.string().required(),
    serviceId: yup.string().required(),
    ignoreTimeClashes: yup.boolean().default(false)
});

export const appointmentClient = {
//     ...baseModelRequest(userUrl, customerRequestAppointmentParser),
}
