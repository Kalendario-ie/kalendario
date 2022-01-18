import {CancelToken} from 'axios';
import {Moment} from 'moment';
import * as moment from 'moment';
import {
    AppointmentAdminResourceModel,
    AppointmentsClient,
    GetAppointmentHistoryResult,
    UpsertAppointmentCommand
} from 'src/app/api/api';
import {ApiListResult} from 'src/app/api/common/api-results';
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

export interface AppointmentClient extends BaseModelRequest<AppointmentAdminResourceModel, UpsertAppointmentCommand, AppointmentsGetParams> {
    history: (id: string, cancelToken?: CancelToken | undefined) => Promise<GetAppointmentHistoryResult>;
}

export const adminAppointmentClient: AppointmentClient = {
    get(params) {
        return client.appointmentsGet(params?.fromDate, params?.toDate, params?.customerId, params?.employeeIds, params?.cancelToken);
    },
    post(body: UpsertAppointmentCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.appointmentsCreate(body, cancelToken);
    },
    put(id: string, command: UpsertAppointmentCommand | undefined, cancelToken?: CancelToken | undefined) {
        return client.appointmentsUpdate(id, command, cancelToken);
    },
    history(id: string, cancelToken?: CancelToken | undefined) {
        return client.appointmentsHistory(id, cancelToken);
    },
    delete(id: string , cancelToken?: CancelToken | undefined) {
        return client.appointmentsDelete(id, cancelToken)
    }
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
        employeeId: appointment.employeeId || '',
        customerId: appointment.customerId || '',
        serviceId: appointment.serviceId || '',
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
