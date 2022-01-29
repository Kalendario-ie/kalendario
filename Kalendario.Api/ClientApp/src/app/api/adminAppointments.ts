import {CancelToken} from 'axios';
import * as moment from 'moment';
import {
    AppointmentAdminResourceModel,
    AppointmentsClient,
    GetAppointmentHistoryResult,
    UpsertAppointmentCommand, UpsertTimeLockCommand
} from 'src/app/api/api';
import baseApiAxios from 'src/app/api/common/clients/base-api';
import {BaseModelRequest} from 'src/app/api/common/clients/base-django-api';
import * as yup from 'yup';

const client = new AppointmentsClient('', baseApiAxios);

export interface AppointmentsGetParams {
    fromDate: string | undefined;
    toDate: string | undefined;
    customerId: string | undefined;
    employeeIds: string[];
    cancelToken?: CancelToken | undefined;
}

export interface AppointmentClient extends BaseModelRequest<AppointmentAdminResourceModel, UpsertAppointmentCommand, AppointmentsGetParams> {
    history: (id: string, cancelToken?: CancelToken | undefined) => Promise<GetAppointmentHistoryResult>;
    createTimeLock: (body: UpsertTimeLockCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<AppointmentAdminResourceModel>;
    updateTimeLock: (id: string, body: UpsertTimeLockCommand | undefined , cancelToken?: CancelToken | undefined) => Promise<AppointmentAdminResourceModel>
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
    },
    createTimeLock(body: UpsertTimeLockCommand | undefined , cancelToken?: CancelToken | undefined) {
        return client.appointmentsCreateTimeLock(body, cancelToken)
    },
    updateTimeLock(id: string, body: UpsertTimeLockCommand | undefined , cancelToken?: CancelToken | undefined) {
        return client.appointmentsUpdateTimeLock(id, body, cancelToken)
    },
}


export function upsertAppointmentCommandParser(appointment: AppointmentAdminResourceModel | null): UpsertAppointmentCommand {
    return appointment == null ? {
        start: moment.utc().toISOString(),
        end: moment.utc().toISOString(),
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

export function upsertTimeLockCommandParser(appointment: AppointmentAdminResourceModel | null): UpsertTimeLockCommand {
    return appointment == null ? {
        start: moment.utc().toISOString(),
        end: moment.utc().toISOString(),
        employeeId: '',
        ignoreTimeClashes: false,
        internalNotes: '',
    } : {
        internalNotes: appointment.internalNotes,
        ignoreTimeClashes: false,
        start: appointment.start,
        end: appointment.end,
        employeeId: appointment.employeeId || '',
    }
}

export const upsertAppointmentCommandValidation = yup.object().shape({
    internalNotes: yup.string(),
    employeeId: yup.string().required(),
    customerId: yup.string().required(),
    serviceId: yup.string().required(),
    ignoreTimeClashes: yup.boolean().default(false)
});


export const upsertTimeLockCommandValidation = yup.object().shape({
    internalNotes: yup.string(),
    employeeId: yup.string().required(),
    ignoreTimeClashes: yup.boolean().default(false)
});

export const appointmentClient = {
//     ...baseModelRequest(userUrl, customerRequestAppointmentParser),
}
